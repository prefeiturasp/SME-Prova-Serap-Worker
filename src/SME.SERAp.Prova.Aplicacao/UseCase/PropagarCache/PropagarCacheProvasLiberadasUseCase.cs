using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class PropagarCacheProvasLiberadasUseCase : AbstractUseCase, IPropagarCacheProvasLiberadasUseCase
    {
        public PropagarCacheProvasLiberadasUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var provas = await mediator.Send(new ObterProvasLiberadasNoPeriodoParaCacheQuery());

            if (provas == null || !provas.Any()) 
                return false;
            
            foreach (var prova in provas)
                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.PropagarCacheProvasLiberadasTratar, prova.Id));

            return true;
        }
    }
}