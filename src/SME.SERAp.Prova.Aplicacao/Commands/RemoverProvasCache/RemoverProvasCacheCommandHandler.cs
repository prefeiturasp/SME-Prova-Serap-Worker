using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;
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
            await repositorioCache.RemoverRedisAsync(CacheChave.ProvasAnosDatasEModalidades);
            // todo await repositorioCache.RemoverRedisAsync(string.Format(CacheChave.QuestaoProvaResumo, request.ProvaId));
            await repositorioCache.RemoverRedisAsync(string.Format(CacheChave.ContextoProvaResumo, request.ProvaId));
            await repositorioCache.RemoverRedisAsync(string.Format(CacheChave.Prova, request.ProvaId));
            return true;
        }
    }
}
