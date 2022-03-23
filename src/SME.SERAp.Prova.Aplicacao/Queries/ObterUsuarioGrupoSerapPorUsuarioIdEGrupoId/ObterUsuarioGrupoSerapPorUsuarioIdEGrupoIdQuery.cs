using MediatR;
using SME.SERAp.Prova.Dominio;


namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUsuarioGrupoSerapPorUsuarioIdEGrupoIdQuery : IRequest<UsuarioGrupoSerapCoreSso>
    {
        public ObterUsuarioGrupoSerapPorUsuarioIdEGrupoIdQuery(long usuarioSerapId, long grupoId)
        {
            UsuarioSerapId = usuarioSerapId;
            GrupoId = grupoId;
        }

        public long UsuarioSerapId { get; set; }
        public long GrupoId { get; set; }
    }
}
