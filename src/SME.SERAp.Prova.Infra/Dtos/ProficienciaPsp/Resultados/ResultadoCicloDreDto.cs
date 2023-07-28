using CsvHelper.Configuration.Attributes;

namespace SME.SERAp.Prova.Infra.Dtos
{
    public class ResultadoCicloDreDto
    {
        [Name("Edicao")]
        public string Edicao { get; set; }

        [Name("AreaConhecimentoID")]
        public int AreaConhecimentoId { get; set; }

        [Name("uad_sigla")]
        public string  DreSigla { get; set; }

        [Name("CicloID")]
        public int CicloId { get; set; }

        [Name("Valor")]
        public string SValor { get; set; }

        [Name("TotalAlunos")]
        public int? TotalAlunos { get; set; }

        [Name("NivelProficienciaID")]
        public int? NivelProficienciaId { get; set; }

        [Name("PercentualAbaixoDoBasico")]
        public string SPercentualAbaixoDoBasico { get; set; }

        [Name("PercentualBasico")]
        public string SPercentualBasico { get; set; }

        [Name("PercentualAdequado")]
        public string SPercentualAdequado { get; set; }

        [Name("PercentualAvancado")]
        public string SPercentualAvancado { get; set; }

        [Name("PercentualAlfabetizado")]
        public string SPercentualAlfabetizado { get; set; }

        public decimal? Valor => SValor.ConvertStringPraDecimalNullPsp();
        public decimal? PercentualAbaixoDoBasico => SPercentualAbaixoDoBasico.ConvertStringPraDecimalNullPsp();
        public decimal? PercentualBasico => SPercentualBasico.ConvertStringPraDecimalNullPsp();
        public decimal? PercentualAdequado => SPercentualAdequado.ConvertStringPraDecimalNullPsp();
        public decimal? PercentualAvancado => SPercentualAvancado.ConvertStringPraDecimalNullPsp();
        public decimal? PercentualAlfabetizado => SPercentualAlfabetizado.ConvertStringPraDecimalNullPsp();
    }
}
