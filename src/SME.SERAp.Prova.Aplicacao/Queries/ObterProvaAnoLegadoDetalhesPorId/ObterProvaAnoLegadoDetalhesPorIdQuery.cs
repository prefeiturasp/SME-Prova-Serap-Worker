using MediatR;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterProvaAnoLegadoDetalhesPorIdQuery : IRequest<IEnumerable<ProvaAnoDetalheDto>>
    {
        public ObterProvaAnoLegadoDetalhesPorIdQuery(long provaId)
        {
            ProvaId = provaId;
        }

        public long ProvaId { get; set; }
    }
}
