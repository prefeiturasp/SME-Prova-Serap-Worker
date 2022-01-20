using MediatR;
using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAudiosPorQuestaoLegadoIdQuery : IRequest<IEnumerable<Arquivo>>
    {
        public ObterAudiosPorQuestaoLegadoIdQuery(long questaoId)
        {
            QuestaoId = questaoId;
        }

        public long QuestaoId { get; set; }
    }
}
