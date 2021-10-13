namespace SME.SERAp.Prova.Infra
{
    public class DetalheQuestaoDto
    {
        public long QuestaoLegadoId { get; set; }
        public int QuestaoLegadoOrdem { get; set; }
        public long ProvaLegadoId { get; set; }

        public DetalheQuestaoDto(long questaoLegadoId, int questaoLegadoOrdem, long provaLegadoId)
        {
            QuestaoLegadoId = questaoLegadoId;
            ProvaLegadoId = provaLegadoId;
            QuestaoLegadoOrdem = questaoLegadoOrdem;
        }
    }
}