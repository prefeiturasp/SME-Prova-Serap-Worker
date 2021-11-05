using System.Collections.Generic;

namespace SME.SERAp.Prova.Infra
{
    public class DetalheAlternativaDto
    {
        
        public long ProvaId { get; set; }
        public long QuestaoId { get; set; }
        public IEnumerable<long> AlternativasId { get; set; }
        
        public DetalheAlternativaDto(long provaId, long questaoId, IEnumerable<long> alternativasId)
        {
            ProvaId = provaId;
            QuestaoId = questaoId;
            AlternativasId = alternativasId;
        }
        
    }
}