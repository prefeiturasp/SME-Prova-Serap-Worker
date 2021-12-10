using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Infra
{
    public class ProvaParaAtualizarDto
    {
        public long ProvaId { get; set; }        
        public int Status { get; set; }
        public DateTime? CriadoEm { get; set; }
        public DateTime? FinalizadoEm { get; set; }

        public long[] IdsProvasAlunos { get; set; }
    }
}
