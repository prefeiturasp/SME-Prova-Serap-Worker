using MediatR;
using SME.SERAp.Prova.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarProvaBIBSyncUseCase : ITratarProvaBIBSyncUseCase
    {
        private readonly IMediator mediator;

        public TratarProvaBIBSyncUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var provaCadernosAlunos = await mediator.Send(new ObterAlunosSemCadernoProvaBibQuery());

            if (provaCadernosAlunos != null && provaCadernosAlunos.Any())
            {
                foreach (var prova in provaCadernosAlunos)
                {
                    await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ProvaBIBTratar, prova));
                }
            }

            return true;
        }
    }
}