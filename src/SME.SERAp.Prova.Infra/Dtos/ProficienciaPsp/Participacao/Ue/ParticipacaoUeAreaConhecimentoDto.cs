using CsvHelper.Configuration.Attributes;

namespace SME.SERAp.Prova.Infra
{
    public class ParticipacaoUeAreaConhecimentoDto : ParticipacaoUeDto
    {
        [Name("AreaConhecimentoID")]
        public int AreaConhecimentoID { get; set; }
    }
}
