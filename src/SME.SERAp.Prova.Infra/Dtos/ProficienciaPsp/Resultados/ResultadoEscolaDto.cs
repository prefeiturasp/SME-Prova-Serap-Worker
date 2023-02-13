using CsvHelper.Configuration.Attributes;

namespace SME.SERAp.Prova.Infra
{
    public class ResultadoEscolaDto : ResultadoPspBaseDto
    {
        [Name("uad_sigla")]
        public string UadSigla { get; set; }

        [Name("esc_codigo")]
        public string EscCodigo { get; set; }
    }
}
