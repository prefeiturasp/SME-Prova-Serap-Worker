using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUePorIdQueryHandler : IRequestHandler<ObterUePorIdQuery, Ue>
    {
        private readonly IRepositorioCache repositorioCache;
        private readonly IRepositorioUe repositorioUe;

        public ObterUePorIdQueryHandler(IRepositorioCache repositorioCache, IRepositorioUe repositorioUe)
        {
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<Ue> Handle(ObterUePorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioCache.ObterRedisAsync($"ue-{request.UeId}", () => repositorioUe.ObterPorIdAsync(request.UeId), 60);
        }
    }
}
