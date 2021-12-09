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
            var ultimaAtualizacao = await mediator.Send(new ObterUltimoExecucaoControleTipoPorTipoQuery(ExecucaoControleTipo.ProvaLegadoSincronizacao));

            try
            {
                SentrySdk.CaptureMessage($"Última Atualização {ultimaAtualizacao.UltimaExecucao}");
                var provaIds = await mediator.Send(new ObterProvaLegadoParaSeremSincronizadasQuery(ultimaAtualizacao.UltimaExecucao));
                SentrySdk.CaptureMessage($"Total de provas para sincronizar {provaIds}");
                foreach (var provaId in provaIds)
                {
                    SentrySdk.CaptureMessage($"Enviando prova {provaId} para tratar");
                    await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ProvaTratar, provaId));
                }              
            }
            catch(Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return false;
            }  
            finally
            {
                await mediator.Send(new ExecucaoControleAtualizarCommand(ultimaAtualizacao));
            }

            return true;
        }
    }
}