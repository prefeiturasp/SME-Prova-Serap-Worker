using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTipoProvaLegadoPorIdQuery : IRequest<TipoProva>
    {
        public long Id { get; private set; }
        public ObterTipoProvaLegadoPorIdQuery(long id)
        {
            Id = id;
        }
    }
}
