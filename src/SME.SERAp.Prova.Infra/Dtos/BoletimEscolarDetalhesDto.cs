namespace SME.SERAp.Prova.Infra.Dtos
{
    public class BoletimEscolarDetalhesDto
    {
        public long UeId { get; set; }
        public long ProvaId { get; set; }
        public int AnoEscolar { get; set; }
        public string Disciplina { get; set; }
        public long DisciplinaId { get; set; }
        public decimal AbaixoBasico { get; set; }
        public decimal AbaixoBasicoPorcentagem { get; set; }
        public decimal Basico { get; set; }
        public decimal BasicoPorcentagem { get; set; }
        public decimal Adequado { get; set; }
        public decimal AdequadoPorcentagem { get; set; }
        public decimal Avancado { get; set; }
        public decimal AvancadoPorcentagem { get; set; }
        public int Total { get; set; }
        public decimal MediaProficiencia { get; set; }
        public string ComponenteCurricular => $"{Disciplina} ({AnoEscolar}º ano)";
        public int NivelUeCodigo { get; set; }
        public string NivelUeDescricao { get; set; }
    }
}
