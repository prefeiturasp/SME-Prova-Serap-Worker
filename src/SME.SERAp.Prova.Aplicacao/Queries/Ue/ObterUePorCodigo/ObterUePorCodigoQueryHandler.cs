using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUePorCodigoQueryHandler : IRequestHandler<ObterUePorCodigoQuery, Ue>
    {
        private readonly IRepositorioUe repositorioUe;
        private readonly IRepositorioCache repositorioCache;

        public ObterUePorCodigoQueryHandler(IRepositorioUe repositorioUe, IRepositorioCache repositorioCache)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<Ue> Handle(ObterUePorCodigoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioCache.ObterRedisAsync($"ue-cod-{request.UeCodigo}", async () => await repositorioUe.ObterUePorCodigo(request.UeCodigo), 60);
        }
    }
}
