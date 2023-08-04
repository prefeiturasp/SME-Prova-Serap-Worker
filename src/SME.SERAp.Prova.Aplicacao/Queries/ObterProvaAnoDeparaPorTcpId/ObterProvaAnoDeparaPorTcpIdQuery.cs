using MediatR;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterProvaAnoDeparaPorTcpIdQuery : IRequest<IEnumerable<TipoCurriculoPeriodoAnoDto>>
    {
        public ObterProvaAnoDeparaPorTcpIdQuery(int[] tcpIds)
        {
            TcpIds = tcpIds;
        }

        public int[] TcpIds { get; set; }
    }
}
