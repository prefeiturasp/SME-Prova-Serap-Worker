using System;

namespace SME.SERAp.Prova.Infra
{
    public class UsuarioGrupoDto
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
