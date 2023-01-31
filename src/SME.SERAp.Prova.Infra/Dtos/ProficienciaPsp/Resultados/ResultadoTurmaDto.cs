using CsvHelper.Configuration.Attributes;

namespace SME.SERAp.Prova.Infra
{
    public class ResultadoTurmaDto : ResultadoPspBaseDto
    {
        [Name("uad_sigla")]
        public string UadSigla { get; set; }

        [Name("esc_codigo")]
        public string EscCodigo { get; set; }

        [Name("tur_codigo")]
        public string TurCodigo { get; set; }

        [Name("tur_id")]
        public int TurId { get; set; }
    }
}
