using System;

namespace SME.SERAp.Prova.Infra
{
    public class TurmaAlunoHistoricoDto : DtoBase
    {
        public long AlunoRa { get; set; }
        public long TurmaId { get; set; }
        public int AnoLetivo { get; set; }
        public long AlunoId { get; set; }
        public DateTime DataMatricula { get; set; }
        public DateTime DataSituacao { get; set; }
    }
}
