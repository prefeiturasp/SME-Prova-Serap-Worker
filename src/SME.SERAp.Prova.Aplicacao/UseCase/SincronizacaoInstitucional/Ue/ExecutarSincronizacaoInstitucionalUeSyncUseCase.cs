using MediatR;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExecutarSincronizacaoInstitucionalUeSyncUseCase : AbstractUseCase, IExecutarSincronizacaoInstitucionalUeSyncUseCase
    {
        public ExecutarSincronizacaoInstitucionalUeSyncUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var dre = mensagemRabbit.ObterObjetoMensagem<DreParaSincronizacaoInstitucionalDto>();

            if (dre == null)
                throw new NegocioException("Não foi possível localizar o código da Dre para tratar o Sync de Ues.");

            var uesSgp = await mediator.Send(new ObterUesSgpPorDreCodigoQuery(dre.DreCodigo));

            if (uesSgp == null || !uesSgp.Any())
                throw new NegocioException("Não foi possível localizar as Ues no Sgp para a sincronização instituicional");

            var uesSgpCodigo = uesSgp.Select(a => a.CodigoUe).Distinct().ToList();

            var uesSerap = await mediator.Send(new ObterUesSerapPorDreCodigoQuery(dre.DreCodigo));
            var uesSerapCodigo = uesSerap.Select(a => a.CodigoUe).Distinct().ToList();

            await TratarInclusao(uesSgp, uesSgpCodigo, uesSerapCodigo, dre.Id);

            await TratarAlteracao(uesSgp, uesSgpCodigo, uesSerap, uesSerapCodigo);

            await PublicarFilaParaTratarTurmas(dre);

            return true;
        }

        private async Task TratarAlteracao(IEnumerable<Ue> todasUesSgp, List<string> todasUesSgpCodigo, IEnumerable<Ue> todasUesSerap, List<string> todasUesSerapCodigo)
        {
            var uesParaAlterarCodigos = todasUesSgpCodigo.Where(a => todasUesSerapCodigo.Contains(a)).ToList();

            if (uesParaAlterarCodigos != null && uesParaAlterarCodigos.Any())
            {
                var uesQuePodemAlterar = todasUesSgp.Where(a => uesParaAlterarCodigos.Contains(a.CodigoUe)).ToList();
                var listaParaAlterar = new List<Ue>();

                foreach (var ueQuePodeAlterar in uesQuePodemAlterar)
                {
                    var ueAntiga = todasUesSerap.FirstOrDefault(a => a.CodigoUe == ueQuePodeAlterar.CodigoUe);
                    if (ueAntiga != null && ueAntiga.DeveAtualizar(ueQuePodeAlterar))
                    {
                        ueAntiga.AtualizarCampos(ueQuePodeAlterar);
                        listaParaAlterar.Add(ueAntiga);
                    }
                }

                if (listaParaAlterar.Any())
                    await mediator.Send(new AlterarUesCommand(listaParaAlterar));
            }
        }
        private async Task TratarInclusao(IEnumerable<Ue> todasUesSgp, List<string> todasUesSgpCodigo, List<string> todasUesSerapCodigo, long dreIdSerap)
        {
            var uesNovasCodigos = todasUesSgpCodigo.Where(a => !todasUesSerapCodigo.Contains(a)).ToList();

            if (uesNovasCodigos != null && uesNovasCodigos.Any())
            {
                var uesNovasParaIncluir = todasUesSgp.Where(a => uesNovasCodigos.Contains(a.CodigoUe)).ToList();

                uesNovasParaIncluir = uesNovasParaIncluir.Select(a => new Ue()
                {
                    CodigoUe = a.CodigoUe,
                    DreId = dreIdSerap,
                    TipoEscola = a.TipoEscola,
                    Nome = a.Nome
                }).ToList();

                await mediator.Send(new InserirUesCommand(uesNovasParaIncluir));
            }
        }

        private async Task PublicarFilaParaTratarTurmas(DreParaSincronizacaoInstitucionalDto dreParaSincronizacaoInstitucionalDto)
        {
            await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.SincronizaEstruturaInstitucionalTurmasSync, dreParaSincronizacaoInstitucionalDto));
        }
    }
}
