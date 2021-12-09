using System.Collections.Generic;
using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterQuestoesPorProvaIdQuery : IRequest<IEnumerable<QuestoesPorProvaIdDto>>
    {
        public long ProvaId { get; private set; }

        public ObterQuestoesPorProvaIdQuery(long provaId)
        {
            ProvaId = provaId;
        }
    }
}