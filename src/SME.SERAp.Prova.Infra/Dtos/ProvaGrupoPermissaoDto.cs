using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Infra
{
  public  class ProvaGrupoPermissaoDto
    {
        public Guid GrupoCoressoId { get; set; }
        public long ProvaLegadoId { get; set;  }
        public bool OcultarProva { get; set; }
    }
}
