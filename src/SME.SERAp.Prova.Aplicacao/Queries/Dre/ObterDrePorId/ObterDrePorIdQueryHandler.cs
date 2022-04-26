using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterDrePorIdQueryHandler : IRequestHandler<ObterDrePorIdQuery, Dre>
    {
        private readonly IRepositorioCache repositorioCache;
        private readonly IRepositorioDre repositorioDre;

        public ObterDrePorIdQueryHandler(IRepositorioCache repositorioCache, IRepositorioDre repositorioDre)
        {
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
            this.repositorioDre = repositorioDre ?? throw new ArgumentNullException(nameof(repositorioDre));
        }

        public async Task<Dre> Handle(ObterDrePorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioCache.ObterRedisAsync($"dre-{request.DreId}", () => repositorioDre.ObterPorIdAsync(request.DreId), 60);
        }
    }
}
