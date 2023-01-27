using CsvHelper.Configuration.Attributes;

namespace SME.SERAp.Prova.Infra
{
    public class ResultadoDreDto : ResultadoPspBaseDto
    {
        [Name("uad_sigla")]
        public string UadSigla { get; set; }
    }
}