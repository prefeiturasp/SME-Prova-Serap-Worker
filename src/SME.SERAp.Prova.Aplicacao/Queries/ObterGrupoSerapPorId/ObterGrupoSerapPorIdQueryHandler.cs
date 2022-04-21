using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterGrupoSerapPorIdQueryHandler : IRequestHandler<ObterGrupoSerapPorIdQuery, GrupoSerapCoreSso>
    {
        private readonly IRepositorioCache repositorioCache;
        private readonly IRepositorioGrupoSerapCoreSso repositorioGrupoSerapCoreSso;

        public ObterGrupoSerapPorIdQueryHandler(IRepositorioCache repositorioCache, IRepositorioGrupoSerapCoreSso repositorioGrupoSerapCoreSso)
        {
            this.repositorioCache = repositorioCache ?? throw new System.ArgumentNullException(nameof(repositorioCache));
            this.repositorioGrupoSerapCoreSso = repositorioGrupoSerapCoreSso ?? throw new System.ArgumentNullException(nameof(repositorioGrupoSerapCoreSso));
        }

        public async Task<GrupoSerapCoreSso> Handle(ObterGrupoSerapPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioCache.ObterRedisAsync($"grp-{request.Id}", ()=> repositorioGrupoSerapCoreSso.ObterPorIdAsync(request.Id), 5);
        }
    }
}
