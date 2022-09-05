using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    internal class ObterProvasBibQueryHandler : IRequestHandler<ObterProvasBibQuery, IEnumerable<ProvaBIBSyncDto>>
    {
        private readonly IRepositorioProva repositorioProva;

        public ObterProvasBibQueryHandler(IRepositorioProva repositorioProva)
        {
            this.repositorioProva = repositorioProva ?? throw new ArgumentNullException(nameof(repositorioProva));
        }

        public async Task<IEnumerable<ProvaBIBSyncDto>> Handle(ObterProvasBibQuery request, CancellationToken cancellationToken)
        {
            return await repositorioProva.ObterProvasBibAsync();
        }
    }
}
