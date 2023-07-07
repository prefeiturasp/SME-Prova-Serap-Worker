using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class MontarQuestaoCompletaPorIdQuery : IRequest<QuestaoCompletaDto>
    {
        public MontarQuestaoCompletaPorIdQuery(long questaoId)
        {
            QuestaoId = questaoId;
        }

        public long QuestaoId { get; }
    }
}
