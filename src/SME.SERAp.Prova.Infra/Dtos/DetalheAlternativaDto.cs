namespace SME.SERAp.Prova.Infra
{
    public class DetalheAlternativaDto
    {
        
        public long ProvaId { get; set; }
        public long QuestaoId { get; set; }
        public long AlternativaId { get; set; }
        
        public DetalheAlternativaDto(long provaId, long questaoId, long alternativaId)
        {
            ProvaId = provaId;
            QuestaoId = questaoId;
            AlternativaId = alternativaId;
        }
        
    }
}