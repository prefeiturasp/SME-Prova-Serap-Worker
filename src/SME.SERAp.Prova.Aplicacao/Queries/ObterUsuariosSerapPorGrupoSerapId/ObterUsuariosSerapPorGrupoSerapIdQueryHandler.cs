using System.Collections.Generic;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System.Threading;
using System.Threading.Tasks;


namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUsuariosSerapPorGrupoSerapIdQueryHandler : IRequestHandler<ObterUsuariosSerapPorGrupoSerapIdQuery, IEnumerable<UsuarioSerapCoreSso>>
    {
        
        private readonly IRepositorioUsuarioSerapCoreSso repositorioUsuarioSerapCoreSso;

        public ObterUsuariosSerapPorGrupoSerapIdQueryHandler(IRepositorioUsuarioSerapCoreSso repositorioUsuarioSerapCoreSso)
        {
            this.repositorioUsuarioSerapCoreSso = repositorioUsuarioSerapCoreSso ?? throw new System.ArgumentNullException(nameof(repositorioUsuarioSerapCoreSso));
        }

        public async Task<IEnumerable<UsuarioSerapCoreSso>> Handle(ObterUsuariosSerapPorGrupoSerapIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioUsuarioSerapCoreSso.ObterPorIdGrupoSerap(request.GrupoSerapId);
        }
    }
}
