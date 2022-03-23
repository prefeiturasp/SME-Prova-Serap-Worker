using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterDrePorIdQuery : IRequest<Dre>
    {
        public ObterDrePorIdQuery(long dreId)
        {
            DreId = dreId;
        }

        public long DreId { get; set; }
    }
}
