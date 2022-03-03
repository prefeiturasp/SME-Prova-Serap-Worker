using MediatR;
using System;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTipoProvaDeficienciaPorTipoProvaLegadoIdQuery : IRequest<IEnumerable<Guid>>
    {
        public long TipoProvaLegadoId { get; private set; }

        public ObterTipoProvaDeficienciaPorTipoProvaLegadoIdQuery(long tipoProvaLegadoId)
        {
            TipoProvaLegadoId = tipoProvaLegadoId;
        }
    }
}
