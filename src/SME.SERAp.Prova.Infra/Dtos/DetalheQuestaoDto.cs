namespace SME.SERAp.Prova.Infra
{
    public class DetalheQuestaoDto
    {
        public long QuestaoLegadoId { get; set; }
        public long ProvaLegadoId { get; set; }

        public DetalheQuestaoDto(long questaoLegadoId, long provaLegadoId)
        {
            QuestaoLegadoId = questaoLegadoId;
            ProvaLegadoId = provaLegadoId;
        }
    }
}