using MediatR;

namespace SME.SERAp.Prova.Aplicacao.Queries.ObterAnoProva
{
    public class ObterAnoProvaQuery : IRequest<int?>
    {
        public long ProvaId { get; set; }
        public ObterAnoProvaQuery(long provaId)
        {
            ProvaId = provaId;
        }
    }
}
