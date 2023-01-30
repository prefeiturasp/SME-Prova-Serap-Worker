namespace SME.SERAp.Prova.Dominio
{
    public class ResultadoPspEntidadeBase
    {
        public string Edicao { get; set; }

        public int AreaConhecimentoID { get; set; }

        public string AnoEscolar { get; set; }

        public decimal? Valor { get; set; }

        public int? TotalAlunos { get; set; }

        public int? NivelProficienciaID { get; set; }

        public decimal? PercentualAbaixoDoBasico { get; set; }

        public decimal? PercentualBasico { get; set; }

        public decimal? PercentualAdequado { get; set; }

        public decimal? PercentualAvancado { get; set; }

        public decimal? PercentualAlfabetizado { get; set; }
    }
}
