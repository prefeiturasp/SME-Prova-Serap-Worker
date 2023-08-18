using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterProvasTaiQueryHandler : IRequestHandler<ObterProvasTaiQuery, IEnumerable<ProvaTaiSyncDto>>
    {
        private readonly IRepositorioProva repositorioProva;

        public ObterProvasTaiQueryHandler(IRepositorioProva repositorioProva)
        {
            this.repositorioProva = repositorioProva ?? throw new ArgumentNullException(nameof(repositorioProva));
        }

        public async Task<IEnumerable<ProvaTaiSyncDto>> Handle(ObterProvasTaiQuery request, CancellationToken cancellationToken)
        {
            return await repositorioProva.ObterProvasTaiAsync();
        }
    }
}