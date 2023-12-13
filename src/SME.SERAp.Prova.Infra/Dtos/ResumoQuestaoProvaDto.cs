using System.Collections.Generic;

namespace SME.SERAp.Prova.Infra
{
    public class ResumoQuestaoProvaDto : DtoBase
    {
        public long ProvaId { get; set; }
        public long QuestaoId { get; set; }
        public long QuestaoLegadoId { get; set; }
        public string Caderno { get; set; }
        public IEnumerable<ResumoAlternativaQuestaoProvaDto> Alternativas { get; set; }
        public int Ordem { get; set; }        
    }
}