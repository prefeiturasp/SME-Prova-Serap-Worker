using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExcluirQuestaoAudioPorQuestaoIdCommand : IRequest<bool>
    {
        public ExcluirQuestaoAudioPorQuestaoIdCommand(long questaoId)
        {
            QuestaoId = questaoId;
        }

        public long QuestaoId { get; set; }
    }
}
