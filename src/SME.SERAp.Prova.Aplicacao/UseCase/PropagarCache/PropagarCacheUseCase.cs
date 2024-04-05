using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class PropagarCacheUseCase : AbstractUseCase, IPropagarCacheUseCase
    {
        public PropagarCacheUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.PropagarCacheParametros));
            await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.PropagarCacheProvasAnos));
            await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.PropagarCacheProvasLiberadas));

            return true;
        }
    }
}