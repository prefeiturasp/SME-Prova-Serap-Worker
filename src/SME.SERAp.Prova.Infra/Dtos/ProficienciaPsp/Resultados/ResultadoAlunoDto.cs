using CsvHelper.Configuration.Attributes;

namespace SME.SERAp.Prova.Infra
{
    public class ResultadoAlunoDto
    {
        [Name("Edicao")]
        public string Edicao { get; set; }
        [Name("AreaConhecimentoID")]
        public int AreaConhecimentoID { get; set; }
        [Name("uad_sigla")]
        public string uad_sigla { get; set; }
        [Name("esc_codigo")]
        public string esc_codigo { get; set; }
        [Name("AnoEscolar")]
        public string AnoEscolar { get; set; }
        [Name("tur_codigo")]
        public string tur_codigo { get; set; }
        [Name("tur_id")]
        public int tur_id { get; set; }
        [Name("alu_matricula")]
        public string alu_matricula { get; set; }
        [Name("alu_nome")]
        public string alu_nome { get; set; }
        [Name("NivelProficienciaID")]
        public int NivelProficienciaID { get; set; }
        [Name("Valor")]
        public string SValor { get; set; }
        [Name("REDQ1")]
        public string SREDQ1 { get; set; }
        [Name("REDQ2")]
        public string SREDQ2 { get; set; }
        [Name("REDQ3")]
        public string SREDQ3 { get; set; }
        [Name("REDQ4")]
        public string SREDQ4 { get; set; }
        [Name("REDQ5")]
        public string SREDQ5 { get; set; }        

        public decimal? Valor => SValor.ConvertStringPraDecimalNullPsp();
        public decimal? REDQ1 => SREDQ1.ConvertStringPraDecimalNullPsp();
        public decimal? REDQ2 => SREDQ2.ConvertStringPraDecimalNullPsp();
        public decimal? REDQ3 => SREDQ3.ConvertStringPraDecimalNullPsp();
        public decimal? REDQ4 => SREDQ4.ConvertStringPraDecimalNullPsp();
        public decimal? REDQ5 => SREDQ5.ConvertStringPraDecimalNullPsp();
    }
}
