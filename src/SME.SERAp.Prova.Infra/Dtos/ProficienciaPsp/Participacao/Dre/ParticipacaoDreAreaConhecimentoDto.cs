using CsvHelper.Configuration.Attributes;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Infra
{
    public class ParticipacaoDreAreaConhecimentoDto : ParticipacaoDreDto
    {
        [Name("AreaConhecimentoID")]
        public int AreaConhecimentoID { get; set; }
    }
}
