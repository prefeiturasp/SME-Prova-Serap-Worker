using CsvHelper.Configuration.Attributes;

namespace SME.SERAp.Prova.Infra
{
    public class ParticipacaoSmeAreaConhecimentoDto : ParticipacaoSmeDto
    {
        [Name("AreaConhecimentoID")]
        public int AreaConhecimentoID { get; set; }
    }
}
