using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class QuestaoArquivoPersistirCommand : IRequest<long>
    {
        public QuestaoArquivoPersistirCommand(long questaoId, long arquivoId)
        {
            QuestaoId = questaoId;
            ArquivoId = arquivoId;
        }

        public long QuestaoId { get; set; }
        public long ArquivoId { get; set; }
    }
}
