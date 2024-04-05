using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterCacheQueryHandler : IRequestHandler<ObterCacheQuery, object>
    {
        private readonly IRepositorioCache repositorioCache;

        public ObterCacheQueryHandler(IRepositorioCache repositorioCache)
        {
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<object> Handle(ObterCacheQuery request, CancellationToken cancellationToken)
        {
            return await repositorioCache.ObterRedisAsync<bool>(request.NomeCache);
        }
    }
}
