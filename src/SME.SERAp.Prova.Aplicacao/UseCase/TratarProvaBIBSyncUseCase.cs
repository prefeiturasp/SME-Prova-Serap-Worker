using MediatR;
using Microsoft.Extensions.Logging;
using SME.SERAp.Prova.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarProvaBIBSyncUseCase : ITratarProvaBIBSyncUseCase
    {
        private readonly ILogger<TratarProvaBIBSyncUseCase> logger;
        private readonly IMediator mediator;

        public TratarProvaBIBSyncUseCase(ILogger<TratarProvaBIBSyncUseCase> logger, IMediator mediator)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger)); ;
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator)); ;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            logger.LogInformation("PROVABIB SYNC - Iniciou");

            try
            {
                var provaCadernosAlunos = await mediator.Send(new ObterAlunosSemCadernoProvaBibQuery());

                logger.LogInformation("PROVABIB SYNC - Total de registros: " + provaCadernosAlunos.Count());

                if (provaCadernosAlunos != null && provaCadernosAlunos.Any())
                {
                    foreach (var prova in provaCadernosAlunos)
                    {
                        await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ProvaBIBTratar, prova));
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "PROVABIB SYNC - ERRO");
                throw ex;
            }

            return true;
        }
    }
}