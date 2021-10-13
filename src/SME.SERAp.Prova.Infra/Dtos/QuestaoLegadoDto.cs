namespace SME.SERAp.Prova.Infra
{
    public class QuestaoLegadoDto
    {
        public long Id  { get; set; }
        public int Ordem  { get; set; }

        public QuestaoLegadoDto(long questaoId, int ordem)
        {
            Id = questaoId;
            Ordem = ordem;
        }

        public QuestaoLegadoDto()
        {
        }
    }
}
