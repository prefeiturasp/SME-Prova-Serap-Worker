using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class VerificaProvaExistePorSerapIdQuery : IRequest<bool>
    {
        public VerificaProvaExistePorSerapIdQuery(long provaId)
        {
            ProvaId = provaId;
        }

        public long ProvaId { get; }
    }
}
