using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class RemoverProvasCacheCommand : IRequest<bool>
    {
        public RemoverProvasCacheCommand()
        {
        }
    }
}
