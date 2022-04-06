using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System.Threading;
using System.Threading.Tasks;


namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUsuarioGrupoSerapPorUsuarioIdEGrupoIdQueryHandler : IRequestHandler<ObterUsuarioGrupoSerapPorUsuarioIdEGrupoIdQuery, UsuarioGrupoSerapCoreSso>
    {

        private readonly IRepositorioUsuarioGrupoSerapCoreSso usuarioGrupoSerapCoreSso;

        public ObterUsuarioGrupoSerapPorUsuarioIdEGrupoIdQueryHandler(IRepositorioUsuarioGrupoSerapCoreSso usuarioGrupoSerapCoreSso)
        {
            this.usuarioGrupoSerapCoreSso = usuarioGrupoSerapCoreSso ?? throw new System.ArgumentNullException(nameof(usuarioGrupoSerapCoreSso));
        }

        public async Task<UsuarioGrupoSerapCoreSso> Handle(ObterUsuarioGrupoSerapPorUsuarioIdEGrupoIdQuery request, CancellationToken cancellationToken)
        {
            return await usuarioGrupoSerapCoreSso.ObterPorUsuarioIdEGrupoIdCoreSso(request.UsuarioSerapId, request.GrupoId);
        }
    }
}
