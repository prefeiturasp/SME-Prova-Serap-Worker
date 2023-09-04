using System.Collections.Generic;
using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterDadosAmostraProvaTaiQuery : IRequest<IEnumerable<AmostraProvaTaiDto>>
    {
        public ObterDadosAmostraProvaTaiQuery(long provaLegadoId)
        {
            ProvaLegadoId = provaLegadoId;
        }

        public long ProvaLegadoId { get; set; }
    }
}
