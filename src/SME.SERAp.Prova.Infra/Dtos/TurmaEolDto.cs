using System;

namespace SME.SERAp.Prova.Infra
{
    public class TurmaEolDto : DtoBase
    {
        public long AlunoRa { get; set; }
        public long CodigoTurma { get; set; }
        public int AnoLetivo { get; set; }
        public DateTime DataMatricula { get; set; }
        public DateTime DataSituacao { get; set; }
    }
}
