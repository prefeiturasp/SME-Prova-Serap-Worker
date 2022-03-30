using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterGrupoSerapPorIdQueryHandler : IRequestHandler<ObterGrupoSerapPorIdQuery, GrupoSerapCoreSso>
    {
        
        private readonly IRepositorioGrupoSerapCoreSso repositorioGrupoSerapCoreSso;

        public ObterGrupoSerapPorIdQueryHandler(IRepositorioGrupoSerapCoreSso repositorioGrupoSerapCoreSso)
        {
            this.repositorioGrupoSerapCoreSso = repositorioGrupoSerapCoreSso ?? throw new System.ArgumentNullException(nameof(repositorioGrupoSerapCoreSso));
        }

        public async Task<GrupoSerapCoreSso> Handle(ObterGrupoSerapPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioGrupoSerapCoreSso.ObterPorIdAsync(request.Id);
        }
    }
}
