using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTipoProvaPorIdQuery : IRequest<TipoProva>
    {
        public long Id { get; private set; }
        public ObterTipoProvaPorIdQuery(long id)
        {
            Id = id;
        }
    }
}
