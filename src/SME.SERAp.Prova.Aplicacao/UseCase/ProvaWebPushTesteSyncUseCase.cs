using System;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ProvaWebPushTesteSyncUseCase : IProvaWebPushTesteSyncUseCase
    {
        private readonly IMediator mediator;

        public ProvaWebPushTesteSyncUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            for (var i = 1; i <= 9; i++)
                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ProvaWebPushTeste, new { ano = i }));

            return true;
        }
    }
}