using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Infra
{
    public class ProvaAdesaoEntityDto
    {
        public long ProvaId { get; set; }
        public string UeCodigo { get; set; }
        public long TurmaId { get; set; }
        public long AlunoRa { get; set; }
        public int TipoTurno { get; set; }
        public int AnoTurma { get; set; }
        public int Modalidade { get; set; }
        public int TipoTurma { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime AtualizadoEm { get; set; }
    }
}
