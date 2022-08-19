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
            var provasbib = await mediator.Send(new ObterProvasBibQuery());
            if (provasbib != null && provasbib.Any())
            {
                foreach (var prova in provasbib)
                {
                    var provaCadernosAlunos = await mediator.Send(new ObterAlunosSemCadernoProvaBibQuery(prova.ProvaId));
                    if (provaCadernosAlunos != null && provaCadernosAlunos.Any())
                    {
                        foreach (var alunos in provaCadernosAlunos)
                        {
                            alunos.TotalCadernos = prova.TotalCadernos;
                            await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ProvaBIBTratar, prova));
                        }
                    }
                }
            }

            return true;
        }
    }
}