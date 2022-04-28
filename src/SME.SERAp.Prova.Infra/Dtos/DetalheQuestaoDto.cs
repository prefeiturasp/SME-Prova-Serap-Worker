namespace SME.SERAp.Prova.Infra
{
    public class DetalheQuestaoDto : DtoBase
    {
        public long QuestaoLegadoId { get; set; }
        public int QuestaoLegadoOrdem { get; set; }
        public long ProvaLegadoId { get; set; }
        public string Caderno { get; set; }

        public DetalheQuestaoDto(long questaoLegadoId, int questaoLegadoOrdem, long provaLegadoId, string caderno)
        {
            QuestaoLegadoId = questaoLegadoId;
            ProvaLegadoId = provaLegadoId;
            QuestaoLegadoOrdem = questaoLegadoOrdem;
            Caderno = caderno;
        }
    }
}