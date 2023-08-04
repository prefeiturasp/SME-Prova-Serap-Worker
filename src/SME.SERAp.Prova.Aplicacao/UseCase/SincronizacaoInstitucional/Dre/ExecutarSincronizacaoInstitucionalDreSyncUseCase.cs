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
        private readonly IServicoLog servicoLog;

        public ExecutarSincronizacaoInstitucionalDreSyncUseCase(IMediator mediator, IServicoLog servicoLog) : base(mediator)
        {
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var todasDresSgp = (await mediator.Send(new ObterDresSgpQuery())).ToList();
                
                if (todasDresSgp == null || !todasDresSgp.Any())
                    throw new NegocioException("Não foi possível localizar as Dres no Sgp para a sincronização institucional.");

                var todasDresSerap = (await mediator.Send(new ObterDresSerapQuery())).ToList();

                await TratarInclusao(todasDresSgp, todasDresSerap);
                await TratarAlteracao(todasDresSgp, todasDresSerap);

                await Tratar();

                return true;
            }
            catch (Exception e)
            {
                servicoLog.Registrar($"Erro ao sincronizar as Dres: {mensagemRabbit.Mensagem}", e);
                throw;
            }
        }

        private async Task TratarInclusao(IList<Dre> todasDresSgp, IEnumerable<Dre> todasDresSerap)
        {
            var todasDresSgpCodigo = todasDresSgp.Select(c => c.CodigoDre).Distinct();
            var todasDresSerapCodigo = todasDresSerap.Select(c => c.CodigoDre).Distinct();
            
            var dresNovasCodigos = todasDresSgpCodigo.Where(a => !todasDresSerapCodigo.Contains(a)).ToList();

            if (dresNovasCodigos.Any())
            {
                var dresNovasParaIncluir = todasDresSgp.Where(a => dresNovasCodigos.Contains(a.CodigoDre)).ToList();
                
                dresNovasParaIncluir = dresNovasParaIncluir.Select(a => new Dre
                {
                    Abreviacao = a.Abreviacao,
                    CodigoDre = a.CodigoDre,
                    Nome = a.Nome
                }).ToList();

                await mediator.Send(new InserirDresCommand(dresNovasParaIncluir));
            }
        }

        private async Task TratarAlteracao(IList<Dre> todasDresSgp, IList<Dre> todasDresSerap)        
        {
            var todasDresSgpCodigo = todasDresSgp.Select(c => c.CodigoDre).Distinct();
            var todasDresSerapCodigo = todasDresSerap.Select(c => c.CodigoDre).Distinct();
            
            var dresParaAlterarCodigos = todasDresSgpCodigo.Where(a => todasDresSerapCodigo.Contains(a)).ToList();

            if (dresParaAlterarCodigos.Any())
            {
                var dresParaQuePodemAlterar = todasDresSgp.Where(a => dresParaAlterarCodigos.Contains(a.CodigoDre)).ToList();
                
                var listaParaAlterar = new List<Dre>();

                foreach (var dreQuePodeAlterar in dresParaQuePodemAlterar)
                {
                    var dreAntiga = todasDresSerap.FirstOrDefault(a => a.CodigoDre == dreQuePodeAlterar.CodigoDre);

                    if (dreAntiga == null || !dreAntiga.DeveAtualizar(dreQuePodeAlterar)) 
                        continue;
                    
                    dreAntiga.AtualizarCampos(dreQuePodeAlterar);
                    listaParaAlterar.Add(dreAntiga);
                }

                if (listaParaAlterar.Any())
                    await mediator.Send(new AlterarDresCommand(listaParaAlterar));
            }
        }

        private async Task Tratar()
        {
            var todasDresSerap = (await mediator.Send(new ObterDresSerapQuery())).ToList();

            foreach (var dre in todasDresSerap)
            {
                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.SincronizaEstruturaInstitucionalDreTratar,
                    new DreParaSincronizacaoInstitucionalDto(dre.Id, dre.CodigoDre)));
            }
        }
    }
}
