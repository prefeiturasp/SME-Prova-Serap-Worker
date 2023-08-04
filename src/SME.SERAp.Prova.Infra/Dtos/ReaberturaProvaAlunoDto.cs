using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Infra
{
  public  class ReaberturaProvaAlunoDto
    {
        public long ProvaId { get; set; }
        public long AlunoRA { get; set; }
        public string LoginCoresso { get; set; }
        public Guid UsuarioCoresso { get; set; }
        public Guid GrupoCoresso { get; set; }

    }
}
