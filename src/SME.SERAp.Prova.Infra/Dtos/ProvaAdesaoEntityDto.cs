using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Infra
{
    public class ProvaAdesaoEntityDto
    {
        public long ProvaId { get; set; }
        public long UeId { get; set; }
        public long TurmaId { get; set; }
        public long AlunoId { get; set; }
        public long AlunoRa { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime AtualizadoEm { get; set; }
    }
}
