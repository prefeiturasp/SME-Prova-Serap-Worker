using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExcluirUsuarioGrupoSerapPorUsuarioIdEGrupoIdCommandHandler : IRequestHandler<ExcluirUsuarioGrupoSerapPorUsuarioIdEGrupoIdCommand, bool>
    {

        private readonly IRepositorioUsuarioGrupoSerapCoreSso usuarioGrupoSerapCoreSso;

        public ExcluirUsuarioGrupoSerapPorUsuarioIdEGrupoIdCommandHandler(IRepositorioUsuarioGrupoSerapCoreSso usuarioGrupoSerapCoreSso)
        {
            this.usuarioGrupoSerapCoreSso = usuarioGrupoSerapCoreSso ?? throw new System.ArgumentNullException(nameof(usuarioGrupoSerapCoreSso));
        }

        public async Task<bool> Handle(ExcluirUsuarioGrupoSerapPorUsuarioIdEGrupoIdCommand request, CancellationToken cancellationToken)
        {
            return await usuarioGrupoSerapCoreSso.RemoverPorUsuarioSerapIdEGrupoId(request.UsuarioSerapId, request.GrupoSerapId);
        }
    }
}
