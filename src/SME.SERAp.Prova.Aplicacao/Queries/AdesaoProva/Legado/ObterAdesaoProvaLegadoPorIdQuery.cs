using System.Collections.Generic;
using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAdesaoProvaLegadoPorIdQuery : IRequest<IEnumerable<ProvaAdesao>>
    {
        public long ProvaLegadoId { get; private set; }
        public ObterAdesaoProvaLegadoPorIdQuery(long provaLegadoId)
        {
            ProvaLegadoId = provaLegadoId;
        }
    }
}
