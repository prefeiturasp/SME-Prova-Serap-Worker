using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class RemoverProvasCacheCommand : IRequest<bool>
    {
        public RemoverProvasCacheCommand(long provaId)
        {
            ProvaId = provaId;
        }

        public long ProvaId { get; set; }
    }
}
