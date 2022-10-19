using System;

namespace SME.SERAp.Prova.Dominio
{
  public  class ProvaGrupoPermissao : EntidadeBase
    {
        public ProvaGrupoPermissao(long provaId, long provaLegadoId, long grupoId, bool ocultarProva)
        {
            ProvaId = provaId;
            ProvaLegadoId = provaLegadoId;
            GrupoId = grupoId;
            OcultarProva = ocultarProva;
            CriadoEm = DateTime.Now;
        }

        public long ProvaId { get; set; }
        public long ProvaLegadoId { get; set;  }
        public long GrupoId { get; set; }
        public bool OcultarProva { get; set; }
        public DateTime? CriadoEm { get; set; }
        public DateTime? AlteradoEm { get; set; }

    }
}