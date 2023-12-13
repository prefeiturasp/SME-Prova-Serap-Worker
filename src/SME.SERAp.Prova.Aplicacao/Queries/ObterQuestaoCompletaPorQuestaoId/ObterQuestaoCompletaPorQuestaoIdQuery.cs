using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterQuestaoCompletaPorQuestaoIdQuery : IRequest<QuestaoCompleta>
    {
        public ObterQuestaoCompletaPorQuestaoIdQuery(long questaoId)
        {
            QuestaoId = questaoId;
        }

        public long QuestaoId { get; }
    }
}