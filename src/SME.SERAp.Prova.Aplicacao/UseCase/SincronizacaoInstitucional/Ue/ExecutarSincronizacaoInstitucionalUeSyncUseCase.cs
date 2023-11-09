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
    public class ExecutarSincronizacaoInstitucionalUeSyncUseCase : AbstractUseCase, IExecutarSincronizacaoInstitucionalUeSyncUseCase
    {
        private readonly IServicoLog servicoLog;

        public ExecutarSincronizacaoInstitucionalUeSyncUseCase(IMediator mediator, IServicoLog servicoLog) : base(mediator)
        {
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var dre = mensagemRabbit.ObterObjetoMensagem<DreParaSincronizacaoInstitucionalDto>();

                if (dre == null)
                    throw new NegocioException("Não foi possível localizar a Dre para sincronizar as Ues.");

                var uesSgp = await mediator.Send(new ObterUesSgpPorDreCodigoQuery(dre.DreCodigo));

                if (uesSgp == null || !uesSgp.Any())
                    throw new NegocioException("Não foi possível localizar as Ues no Sgp para a sincronização instituicional");

                // seta o id da dre do serap para as ues
                foreach (var u in uesSgp)
                    u.DreId = dre.Id;

                var codigos = uesSgp.Select(t => t.CodigoUe).Distinct().ToArray();
                var uesSerap = await mediator.Send(new ObterUesSerapPorCodigosQuery(codigos));

                await TratarInclusao(uesSgp, uesSerap);
                await TratarAlteracao(uesSgp, uesSerap);

                await Tratar(dre);

                return true;
            }
            catch (Exception e)
            {
                servicoLog.Registrar($"Erro ao sincronizar as UEs: {mensagemRabbit.Mensagem}", e);
                throw;
            }
        }

        private async Task TratarAlteracao(IEnumerable<Ue> todasUesSgp, IEnumerable<Ue> todasUesSerap)
        {
            var todasUesSgpCodigo = todasUesSgp.Select(c => c.CodigoUe).Distinct();
            var todasUesSerapCodigo = todasUesSerap.Select(c => c.CodigoUe).Distinct();

            var uesParaAlterarCodigos = todasUesSgpCodigo.Where(a => todasUesSerapCodigo.Contains(a)).ToList();

            if (uesParaAlterarCodigos.Any())
            {
                var uesQuePodemAlterar = todasUesSgp.Where(a => uesParaAlterarCodigos.Contains(a.CodigoUe)).ToList();

                var listaParaAlterar = new List<Ue>();

                foreach (var ueQuePodeAlterar in uesQuePodemAlterar)
                {
                    var ueAntiga = todasUesSerap.FirstOrDefault(a => a.CodigoUe == ueQuePodeAlterar.CodigoUe);

                    if (ueAntiga == null || !ueAntiga.DeveAtualizar(ueQuePodeAlterar))
                        continue;

                    ueAntiga.AtualizarCampos(ueQuePodeAlterar);
                    listaParaAlterar.Add(ueAntiga);
                }

                if (listaParaAlterar.Any())
                    await mediator.Send(new AlterarUesCommand(listaParaAlterar));
            }
        }

        private async Task TratarInclusao(IEnumerable<Ue> todasUesSgp, IEnumerable<Ue> todasUesSerap)
        {
            var todasUesSgpCodigo = todasUesSgp.Select(c => c.CodigoUe).Distinct();
            var todasUesSerapCodigo = todasUesSerap.Select(c => c.CodigoUe).Distinct();

            var uesNovasCodigos = todasUesSgpCodigo.Where(a => !todasUesSerapCodigo.Contains(a)).ToList();

            if (uesNovasCodigos.Any())
            {
                var uesNovasParaIncluir = todasUesSgp.Where(a => uesNovasCodigos.Contains(a.CodigoUe)).ToList();

                uesNovasParaIncluir = uesNovasParaIncluir.Select(a => new Ue
                {
                    CodigoUe = a.CodigoUe,
                    DreId = a.DreId,
                    TipoEscola = a.TipoEscola,
                    Nome = a.Nome
                }).ToList();

                await mediator.Send(new InserirUesCommand(uesNovasParaIncluir));
            }
        }

        private async Task Tratar(DreParaSincronizacaoInstitucionalDto dre)
        {
            var todasUesSerap = (await mediator.Send(new ObterUesSerapPorDreCodigoQuery(dre.DreCodigo))).ToList();

            foreach (var ue in todasUesSerap)
            {
                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.SincronizaEstruturaInstitucionalUeTratar,
                    new UeParaSincronizacaoInstitucionalDto(ue.Id, ue.CodigoUe, dre.Id, dre.DreCodigo)));
            }
        }
    }
}
