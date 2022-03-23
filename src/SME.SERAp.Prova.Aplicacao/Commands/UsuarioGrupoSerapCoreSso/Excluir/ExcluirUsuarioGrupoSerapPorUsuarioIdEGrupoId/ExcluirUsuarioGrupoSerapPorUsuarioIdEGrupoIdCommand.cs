using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExcluirUsuarioGrupoSerapPorUsuarioIdEGrupoIdCommand : IRequest<bool>
    {
        public ExcluirUsuarioGrupoSerapPorUsuarioIdEGrupoIdCommand(long usuarioSerapId, long grupoSerapId)
        {
            UsuarioSerapId = usuarioSerapId;
            GrupoSerapId = grupoSerapId;
        }

        public long UsuarioSerapId { get; set; }
        public long GrupoSerapId { get; set; }
    }
}
