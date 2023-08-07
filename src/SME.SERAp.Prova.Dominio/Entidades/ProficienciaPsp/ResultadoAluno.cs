namespace SME.SERAp.Prova.Dominio
{
    public class ResultadoAluno
    {
        public string Edicao { get; set; }
        public int AreaConhecimentoID { get; set; }
        public string uad_sigla { get; set; }
        public string esc_codigo { get; set; }
        public string AnoEscolar { get; set; }
        public string tur_codigo { get; set; }
        public int tur_id { get; set; }
        public string alu_matricula { get; set; }
        public string alu_nome { get; set; }
        public int NivelProficienciaID { get; set; }
        public decimal? Valor { get; set; }
        public decimal? REDQ1 { get; set; }
        public decimal? REDQ2 { get; set; }
        public decimal? REDQ3 { get; set; }
        public decimal? REDQ4 { get; set; }
        public decimal? REDQ5 { get; set; }
    }
}
