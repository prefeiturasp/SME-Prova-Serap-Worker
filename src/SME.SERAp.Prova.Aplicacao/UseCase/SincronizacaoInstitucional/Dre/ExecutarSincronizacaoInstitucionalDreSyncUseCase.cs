using MediatR;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExecutarSincronizacaoInstitucionalDreSyncUseCase : AbstractUseCase, IExecutarSincronizacaoInstitucionalDreSyncUseCase
    {
        private readonly IServicoLog serviceLog;
        public ExecutarSincronizacaoInstitucionalDreSyncUseCase(IMediator mediator, IServicoLog serviceLog) : base(mediator)
        {
            this.serviceLog = serviceLog ?? throw new ArgumentNullException(nameof(serviceLog));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {

          
            var todasDresSgp = await mediator.Send(new ObterDresSgpQuery());
            var todasDresSgpCodigo = todasDresSgp.Select(a => a.CodigoDre).ToList();

            if (todasDresSgp == null || !todasDresSgp.Any())
            {
                throw new NegocioException("Não foi possível localizar as Dres no Sgp para a sincronização instituicional");
            }

            var todasDresSerap = await mediator.Send(new ObterDresSerapQuery());
            var todasDresSerapCodigo = todasDresSerap.Select(a => a.CodigoDre).ToList();

            await TratarInclusao(todasDresSgp, todasDresSgpCodigo, todasDresSerapCodigo);

            await TratarAlteracao(todasDresSgp, todasDresSgpCodigo, todasDresSerap, todasDresSerapCodigo);

            await PublicarFilaParaTratarUes();

            return true;

            }
            catch (Exception ex)
            {
                serviceLog.Registrar($"Erro ao sincronizar as Dres: {mensagemRabbit.Mensagem}", ex);
                throw;
            }
        }

        private async Task TratarInclusao(IEnumerable<Dre> todasDresSgp, List<string> todasDresSgpCodigo, List<string> todasDresSerapCodigo)
        {
            var dresNovasCodigos = todasDresSgpCodigo.Where(a => !todasDresSerapCodigo.Contains(a)).ToList();

            if (dresNovasCodigos != null && dresNovasCodigos.Any())
            {
                var dresNovasParaIncluir = todasDresSgp.Where(a => dresNovasCodigos.Contains(a.CodigoDre)).ToList();
                dresNovasParaIncluir = dresNovasParaIncluir.Select(a => new Dre()
                {
                    Abreviacao = a.Abreviacao,
                    CodigoDre = a.CodigoDre,
                    Nome = a.Nome
                }).ToList();

                await mediator.Send(new InserirDresCommand(dresNovasParaIncluir));
            }
        }

        private async Task TratarAlteracao(IEnumerable<Dre> todasDresSgp, List<string> todasDresSgpCodigo, IEnumerable<Dre> todasDresSerap, List<string> todasDresSerapCodigo)
        {
            var dresParaAlterarCodigos = todasDresSgpCodigo.Where(a => todasDresSerapCodigo.Contains(a)).ToList();

            if (dresParaAlterarCodigos != null && dresParaAlterarCodigos.Any())
            {
                var dresParaQuePodemAlterar = todasDresSgp.Where(a => dresParaAlterarCodigos.Contains(a.CodigoDre)).ToList();
                var listaParaAlterar = new List<Dre>();

                foreach (var dreQuePodeAlterar in dresParaQuePodemAlterar)
                {
                    var dreAntiga = todasDresSerap.FirstOrDefault(a => a.CodigoDre == dreQuePodeAlterar.CodigoDre);
                    if (dreAntiga != null && dreAntiga.DeveAtualizar(dreQuePodeAlterar))
                    {
                        dreAntiga.AtualizarCampos(dreQuePodeAlterar);
                        listaParaAlterar.Add(dreAntiga);
                    }
                }

                if (listaParaAlterar.Any())
                    await mediator.Send(new AlterarDresCommand(listaParaAlterar));
            }
        }

        private async Task PublicarFilaParaTratarUes()
        {
            var todasDresSerap = await mediator.Send(new ObterDresSerapQuery());

            foreach (var dreParaTratarUes in todasDresSerap)
            {
                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.SincronizaEstruturaInstitucionalUesSync, new DreParaSincronizacaoInstitucionalDto(dreParaTratarUes.Id, dreParaTratarUes.CodigoDre)));
            }
        }
    }
}
