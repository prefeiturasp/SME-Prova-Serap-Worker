using CsvHelper.Configuration.Attributes;

namespace SME.SERAp.Prova.Infra
{
    public class ResultadoSmeDto
    {

        [Name("Edicao")]
        public string Edicao { get; set; }

        [Name("AreaConhecimentoID")]
        public int AreaConhecimentoID { get; set; }

        [Name("AnoEscolar")]
        public string AnoEscolar { get; set; }

        [Name("Valor")]
        public string _valor { get; set; }

        [Name("TotalAlunos")]
        public int TotalAlunos { get; set; }

        [Name("NivelProficienciaID")]
        public int NivelProficienciaID { get; set; }

        [Name("PercentualAbaixoDoBasico")]
        public string _percentualAbaixoDoBasico { get; set; }

        [Name("PercentualBasico")]
        public string _percentualBasico { get; set; }

        [Name("PercentualAdequado")]
        public string _percentualAdequado { get; set; }

        [Name("PercentualAvancado")]
        public string _percentualAvancado { get; set; }


        public decimal? Valor { get { return _valor.ConvertStringPraDecimalNullPsp(); } }
        public decimal? PercentualAbaixoDoBasico { get { return _percentualAbaixoDoBasico.ConvertStringPraDecimalNullPsp(); } }
        public decimal? PercentualBasico { get { return _percentualBasico.ConvertStringPraDecimalNullPsp(); } }
        public decimal? PercentualAdequado { get { return _percentualAdequado.ConvertStringPraDecimalNullPsp(); } }
        public decimal? PercentualAvancado { get { return _percentualAvancado.ConvertStringPraDecimalNullPsp(); } }

    }
}