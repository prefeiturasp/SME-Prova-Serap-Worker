using CsvHelper.Configuration.Attributes;

namespace SME.SERAp.Prova.Infra 
{ 
    public class ParticipacaoTurmaAreaConhecimentoDto : ParticipacaoTurmaDto
    {
        [Name("AreaConhecimentoID")]
        public int AreaConhecimentoID { get; set; }
    }
}
