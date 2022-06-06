using MediatR;
using SME.SERAp.Prova.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class RabbitDeadletterSerapSyncUseCase : IRabbitDeadletterSerapSyncUseCase
    {
        private readonly IMediator mediator;

        public RabbitDeadletterSerapSyncUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            await EnviarParaFilaSync(mensagemRabbit);
            return await Task.FromResult(true);
        }

        private async Task EnviarParaFilaSync(MensagemRabbit mensagem)
        {
            foreach (var fila in typeof(RotasRabbit).ObterConstantesPublicas<string>())
            {
                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.FilaDeadletterTratar, fila));
            }
        }
    }

}
