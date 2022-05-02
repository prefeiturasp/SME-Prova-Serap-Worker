namespace SME.SERAp.Prova.Infra
{
    public class BuscarPorProvaIdEQuestaoIdDto : DtoBase
    {
        public long ProvaId  { get; set; }
        public long QuestaoId  { get; set; }

        public BuscarPorProvaIdEQuestaoIdDto(long provaId, long questaoId)
        {
            ProvaId = provaId;
            QuestaoId = questaoId;
        }

        public BuscarPorProvaIdEQuestaoIdDto()
        {
        }
    }
}
