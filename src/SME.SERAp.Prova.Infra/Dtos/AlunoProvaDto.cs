namespace SME.SERAp.Prova.Infra
{
    public class AlunoProvaDto : DtoBase
    {
        public long AlunoId { get; set; }
        public long AlunoRa { get; set; }
        public long ProvaId { get; set; }
        public long ProvaLegadoId { get; set; }
        public long? DisciplinaId { get; set; }
    }
}
