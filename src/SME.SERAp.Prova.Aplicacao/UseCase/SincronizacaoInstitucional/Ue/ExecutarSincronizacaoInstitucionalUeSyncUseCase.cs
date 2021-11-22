using MediatR;
using Sentry;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Dtos;
using SME.SERAp.Prova.Infra.Exceptions;
using System;
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

            var ues = await mediator.Send(new ObterUesSgpPorDreCodigoQuery(dre.DreCodigo));

            if (ues == null || !ues.Any())
                throw new NegocioException("Não foi possível localizar as Ues no Sgp para a sincronização instituicional");


            foreach (var ue in ues)
            {
                try
                {
                    ue.DreId = dre.Id;
                    var publicarTratamentoDre = await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.SincronizaEstruturaInstitucionalUeTratar, ue));
                    if (!publicarTratamentoDre)
                    {
                        var mensagem = $"Não foi possível inserir a Ue : {ue.Nome} na fila de sync.";
                        SentrySdk.CaptureMessage(mensagem);
                    }
                }
                catch (Exception ex)
                {
                    SentrySdk.CaptureException(ex);
                }
            }

            return true;
        }
    }
}
