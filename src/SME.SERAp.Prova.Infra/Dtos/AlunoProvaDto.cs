using System;

namespace SME.SERAp.Prova.Infra
{
    public class AlunoProvaDto : DtoBase
    {
        public long ProvaId { get; set; }
        public long ProvaLegadoId { get; set; }
        public long? DisciplinaId { get; set; }
        public DateTime UltimaAtualizacao { get; set; }
        public long AlunoId { get; set; }
        public long AlunoRa { get; set; }
        public string Ano { get; set; }
        public int AnoLetivo { get; set; }
        public long TurmaId { get; set; }
        public long UeId { get; set; }
        public string UeCodigo { get; set; }        
        public long DreId { get; set; }
    }
}
