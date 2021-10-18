using MediatR;
using Sentry;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarProvasLegadoSyncUseCase : ITratarProvasLegadoSyncUseCase
    {
        private readonly IMediator mediator;


        public TratarProvasLegadoSyncUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var ultimaAtualizacao = await mediator.Send(new ObterUltimoExecucaoControleTipoPorTipoQuery(ExecucaoControleTipo.ProvaLegadoSincronizacao));

                var provaIds = await mediator.Send(new ObterProvaLegadoParaSeremSincronizadasQuery(ultimaAtualizacao.UltimaExecucao));
                foreach (var provaId in provaIds)
                {
                    await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ProvaTratar, provaId));
                }

                await mediator.Send(new ExecucaoControleAtualizarCommand(ultimaAtualizacao));
            }
            catch(Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return false;
            }           

            return true;
        }
    }
}