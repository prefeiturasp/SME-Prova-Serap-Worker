namespace SME.SERAp.Prova.Infra
{
    public class QuestaoLegadoDto : DtoBase
    {
        public long Id  { get; set; }
        public int Ordem  { get; set; }
        public string Caderno { get; set; }

        public QuestaoLegadoDto(long questaoId, int ordem, string caderno)
        {
            Id = questaoId;
            Ordem = ordem;
            Caderno = caderno;
        }

        public QuestaoLegadoDto()
        {
        }
    }
}
