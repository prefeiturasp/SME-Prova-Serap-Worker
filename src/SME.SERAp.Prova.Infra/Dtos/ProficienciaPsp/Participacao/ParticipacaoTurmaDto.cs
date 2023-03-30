using CsvHelper.Configuration.Attributes;


namespace SME.SERAp.Prova.Infra
{
    public class ParticipacaoTurmaDto
    {
        [Name("Edicao")]
        public string Edicao { get; set; }
        [Name("uad_sigla")]
        public string uad_sigla { get; set; }
        [Name("esc_codigo")]
        public string esc_codigo { get; set; }
        [Name("AnoEscolar")]
        public string AnoEscolar { get; set; }
        [Name("tur_codigo")]
        public string tur_codigo { get; set; }
        [Name("tur_id")]
        public long tur_id { get; set; }
        [Name("TotalPrevisto")]
        public int TotalPrevisto { get; set; }
        [Name("TotalPresente")]
        public int TotalPresente { get; set; }
        [Name("PercentualParticipacao")]
        public string _percentualParticipacao { get; set; }

        public decimal? PercentualParticipacao { get { return _percentualParticipacao.ConvertStringPraDecimalNullPsp(); } }


    }
}
