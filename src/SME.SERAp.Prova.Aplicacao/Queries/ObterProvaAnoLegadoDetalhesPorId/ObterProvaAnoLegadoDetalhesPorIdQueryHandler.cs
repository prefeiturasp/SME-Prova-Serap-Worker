using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterProvaAnoLegadoDetalhesPorIdQueryHandler : IRequestHandler<ObterProvaAnoLegadoDetalhesPorIdQuery, IEnumerable<ProvaAnoDetalheDto>>
    {

        private readonly IRepositorioProvaLegado repositorioProvaLegado;

        public ObterProvaAnoLegadoDetalhesPorIdQueryHandler(IRepositorioProvaLegado repositorioProvaLegado)
        {
            this.repositorioProvaLegado = repositorioProvaLegado ?? throw new ArgumentNullException(nameof(repositorioProvaLegado));
        }

        public async Task<IEnumerable<ProvaAnoDetalheDto>> Handle(ObterProvaAnoLegadoDetalhesPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioProvaLegado.ObterProvaAnoDetalhesPorId(request.ProvaId);
        }
    }
}
