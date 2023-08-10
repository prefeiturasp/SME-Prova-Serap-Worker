using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarProvaTaiSyncUseCase : ITratarProvaTaiSyncUseCase
    {
        private readonly IMediator mediator;

        public TratarProvaTaiSyncUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var provasTai = (await mediator.Send(new ObterProvasTaiQuery())).ToList();

            if (!provasTai.Any())
                return false;

            foreach (var provaTai in provasTai)
                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ProvaTaiTratar, provaTai));

            return true;
        }
    }
}