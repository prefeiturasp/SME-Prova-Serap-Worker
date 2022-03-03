using MediatR;
using SME.SERAp.Prova.Dominio;
using System;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTipoDeficienciaPorLegadoIdQuery : IRequest<TipoDeficiencia>
    {
        public Guid LegadoId { get; private set; }

        public ObterTipoDeficienciaPorLegadoIdQuery(Guid legadoId)
        {
            LegadoId = legadoId;
        }
    }
}
