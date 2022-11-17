using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterProvaOrigemCadernoQuery : IRequest<long?>
    {
        public ObterProvaOrigemCadernoQuery(long provaId)
        {
            ProvaId = provaId;
        }

        public long ProvaId { get; set; }
    }
}
