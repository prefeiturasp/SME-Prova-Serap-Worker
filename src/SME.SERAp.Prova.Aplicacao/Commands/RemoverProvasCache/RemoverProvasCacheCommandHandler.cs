using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class RemoverProvasCacheCommandHandler : IRequestHandler<RemoverProvasCacheCommand, bool>
    {
        private readonly IRepositorioCache repositorioCache;

        public RemoverProvasCacheCommandHandler(IRepositorioCache repositorioCache)
        {
            this.repositorioCache = repositorioCache ?? throw new System.ArgumentNullException(nameof(repositorioCache));
        }
        public async Task<bool> Handle(RemoverProvasCacheCommand request, CancellationToken cancellationToken)
        {
            await repositorioCache.RemoverRedisAsync("pas");
            return true;
        }
    }
}
