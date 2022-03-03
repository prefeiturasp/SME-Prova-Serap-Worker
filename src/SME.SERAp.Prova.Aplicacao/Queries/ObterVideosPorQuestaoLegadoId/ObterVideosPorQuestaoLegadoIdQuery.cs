using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterVideosPorQuestaoLegadoIdQuery : IRequest<IEnumerable<QuestaoVideoDto>>
    {
        public ObterVideosPorQuestaoLegadoIdQuery(long questaoId)
        {
            QuestaoId = questaoId;
        }

        public long QuestaoId { get; set; }
    }
}
