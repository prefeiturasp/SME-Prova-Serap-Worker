using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterGruposSerapCoreSsoQueryHandler : IRequestHandler<ObterGruposSerapCoreSsoQuery, IEnumerable<GrupoSerapCoreSso>>
    {
        
        private readonly IRepositorioGrupoSerapCoreSso repositorioGrupoSerapCoreSso;

        public ObterGruposSerapCoreSsoQueryHandler(IRepositorioGrupoSerapCoreSso repositorioGrupoSerapCoreSso)
        {
            this.repositorioGrupoSerapCoreSso = repositorioGrupoSerapCoreSso ?? throw new System.ArgumentNullException(nameof(repositorioGrupoSerapCoreSso));
        }

        public async Task<IEnumerable<GrupoSerapCoreSso>> Handle(ObterGruposSerapCoreSsoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioGrupoSerapCoreSso.ObterGruposSerapCoreSso();
        }
    }
}
