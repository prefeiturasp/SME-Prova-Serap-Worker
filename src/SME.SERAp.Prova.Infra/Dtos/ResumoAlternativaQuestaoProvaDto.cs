namespace SME.SERAp.Prova.Infra
{
    public class ResumoAlternativaQuestaoProvaDto : DtoBase
    {
        public long QuestaoId { get; set; }
        public long AlternativaId { get; set; }
        public long AlternativaLegadoId { get; set; }
        public int Ordem { get; set; }        
    }
}