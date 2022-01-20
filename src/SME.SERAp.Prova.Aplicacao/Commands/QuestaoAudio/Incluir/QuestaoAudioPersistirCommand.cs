using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class QuestaoAudioPersistirCommand : IRequest<long>
    {
        public QuestaoAudioPersistirCommand(long questaoId, long arquivoId)
        {
            QuestaoId = questaoId;
            ArquivoId = arquivoId;
        }

        public long QuestaoId { get; set; }
        public long ArquivoId { get; set; }
    }
}
