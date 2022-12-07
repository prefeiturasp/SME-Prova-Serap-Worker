namespace SME.SERAp.Prova.Infra
{
    public class ProficienciaAlunoProvaTaiDto
    {
        public long ProvaId { get; set; }
        public long AlunoId { get; set; }
        public long AlunoRa { get; set; }
        public long? DisciplinaId { get; set; }
        public long Proficiencia { get; set; }
        public int Origem { get; set; }
        public int Tipo { get; set; }

    }
}
