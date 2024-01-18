using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterDrePorCodigoQueryHandler : IRequestHandler<ObterDrePorCodigoQuery, Dre>
    {
        private readonly IRepositorioDre repositorioDre;
        private readonly IRepositorioCache repositorioCache;

        public ObterDrePorCodigoQueryHandler(IRepositorioDre repositorioDre, IRepositorioCache repositorioCache)
        {
            this.repositorioDre = repositorioDre ?? throw new ArgumentNullException(nameof(repositorioDre));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<Dre> Handle(ObterDrePorCodigoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioCache.ObterRedisAsync($"dre-cod-{request.DreCodigo}", async () => await repositorioDre.ObterDREPorCodigo(request.DreCodigo), 60);
        }
    }
}
