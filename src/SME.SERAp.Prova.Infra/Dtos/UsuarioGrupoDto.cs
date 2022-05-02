
namespace SME.SERAp.Prova.Infra
{
    public class UsuarioGrupoDto : DtoBase
    {

        public long IdGrupo { get; set; }
        public UsuarioCoreSsoDto UsuarioCoreSso { get; set; }

        public UsuarioGrupoDto(long idGrupo, UsuarioCoreSsoDto usuarioCoreSso)
        {
            IdGrupo = idGrupo;
            UsuarioCoreSso = usuarioCoreSso;
        }
    }
}
