using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterProvaAnoDeparaPorTcpIdQueryHandler : IRequestHandler<ObterProvaAnoDeparaPorTcpIdQuery, IEnumerable<TipoCurriculoPeriodoAnoDto>>
    {
        private readonly IRepositorioProvaAno repositorioProvaAno;

        public ObterProvaAnoDeparaPorTcpIdQueryHandler(IRepositorioProvaAno repositorioProvaAno)
        {
            this.repositorioProvaAno = repositorioProvaAno ?? throw new ArgumentNullException(nameof(repositorioProvaAno));
        }

        public async Task<IEnumerable<TipoCurriculoPeriodoAnoDto>> Handle(ObterProvaAnoDeparaPorTcpIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioProvaAno.ObterProvaAnoPorTipoCurriculoPeriodoId(request.TcpIds);
        }
    }
}
