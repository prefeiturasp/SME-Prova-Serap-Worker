using System.Collections.Generic;
using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTipoProvaPorLegadoIdQuery : IRequest<TipoProva>
    {
        public long LegadoId { get; private set; }
        public ObterTipoProvaPorLegadoIdQuery(long legadoId)
        {
            LegadoId = legadoId;
        }
    }
}
