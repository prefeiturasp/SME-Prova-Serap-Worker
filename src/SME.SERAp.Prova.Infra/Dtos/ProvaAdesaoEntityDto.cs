using System;

namespace SME.SERAp.Prova.Infra
{
    public class ProvaAdesaoEntityDto : DtoBase
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
