
namespace SME.SERAp.Prova.Dominio
{
    public class QuestaoAudio : EntidadeBase
    {
        public QuestaoAudio()
        {

        }
        public QuestaoAudio(long arquivoId, long questaoId)
        {
            ArquivoId = arquivoId;
            QuestaoId = questaoId;
        }

        public long ArquivoId { get; set; }
        public long QuestaoId { get; set; }
    }
}
