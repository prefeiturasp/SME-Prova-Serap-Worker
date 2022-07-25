using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class RemoverQuestaoCacheCommandHandler : IRequestHandler<RemoverQuestaoCacheCommand, bool>
    {
        private readonly IRepositorioCache repositorioCache;

        public RemoverQuestaoCacheCommandHandler(IRepositorioCache repositorioCache)
        {
            this.repositorioCache = repositorioCache ?? throw new ArgumentException(nameof(repositorioCache));
        }

        public async Task<bool> Handle(RemoverQuestaoCacheCommand request, CancellationToken cancellationToken)
        {
            await repositorioCache.RemoverRedisAsync(string.Format(CacheChave.QuestaoCompleta, request.QuestaoId));
            await repositorioCache.RemoverRedisAsync(string.Format(CacheChave.QuestaoCompletaLegado, request.QuestaoLegadoId));
            return true;
        }
    }
}
