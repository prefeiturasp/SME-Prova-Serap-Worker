using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class SalvarCacheCommandCommandHandler : IRequestHandler<SalvarCacheCommandCommand, bool>
    {
        private readonly IRepositorioCache repositorioCache;

        public SalvarCacheCommandCommandHandler(IRepositorioCache repositorioCache)
        {
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<bool> Handle(SalvarCacheCommandCommand request, CancellationToken cancellationToken)
        {
            await repositorioCache.SalvarRedisAsync(request.NomeCache, request.Valor);
            return true;
        }
    }
}
