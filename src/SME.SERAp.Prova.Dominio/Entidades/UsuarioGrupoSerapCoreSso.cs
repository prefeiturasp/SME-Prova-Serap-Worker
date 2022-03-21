using System;

namespace SME.SERAp.Prova.Dominio
{
    public class UsuarioGrupoSerapCoreSso : EntidadeBase
    {
        public long IdUsuarioSerapCoreSso { get; set; }
        public long IdGrupoSerapCoreSso { get; set; }
        public DateTime CriadoEm { get; set; }

        public UsuarioGrupoSerapCoreSso()
        {

        }

        public UsuarioGrupoSerapCoreSso(long idGrupoSerapCoreSso)
        {
            IdGrupoSerapCoreSso = idGrupoSerapCoreSso;
            CriadoEm = DateTime.Now;
        }
    }
}
