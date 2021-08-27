using System.Collections.Generic;
using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterDetalheQuestoesPorProvaIdQuery : IRequest<QuestoesPorProvaIdDto>
    {
        public long ProvaLegadoId { get; private set; }
        public long QuestaoLegadoId { get; private set; }

        public ObterDetalheQuestoesPorProvaIdQuery(long provaLegadoId, long questaoLegadoId)
        {
            ProvaLegadoId = provaLegadoId;
            QuestaoLegadoId = questaoLegadoId;
        }
    }
}