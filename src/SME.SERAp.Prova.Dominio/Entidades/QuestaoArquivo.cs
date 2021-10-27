namespace SME.SERAp.Prova.Dominio
{
    public class QuestaoArquivo : EntidadeBase
    {
        public QuestaoArquivo()
        {

        }
        public QuestaoArquivo(long arquivoId, long questaoId)
        {
            ArquivoId = arquivoId;
            QuestaoId = questaoId;
        }

        public long ArquivoId { get; set; }
        public long QuestaoId { get; set; }

    }
}
