using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUsuarioSerapPorIdQueryHandler : IRequestHandler<ObterUsuarioSerapPorIdQuery, UsuarioSerapCoreSso>
    {
        private readonly IRepositorioCache repositorioCache;
        private readonly IRepositorioUsuarioSerapCoreSso repositorioUsuarioSerapCoreSso;

        public ObterUsuarioSerapPorIdQueryHandler(IRepositorioCache repositorioCache, IRepositorioUsuarioSerapCoreSso repositorioUsuarioSerapCoreSso)
        {
            this.repositorioCache = repositorioCache ?? throw new System.ArgumentNullException(nameof(repositorioCache));
            this.repositorioUsuarioSerapCoreSso = repositorioUsuarioSerapCoreSso ?? throw new System.ArgumentNullException(nameof(repositorioUsuarioSerapCoreSso));
        }

        public async Task<UsuarioSerapCoreSso> Handle(ObterUsuarioSerapPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioCache.ObterRedisAsync($"usu-{request.Id}", () => repositorioUsuarioSerapCoreSso.ObterPorIdAsync(request.Id), 60);
        }

    }
}
