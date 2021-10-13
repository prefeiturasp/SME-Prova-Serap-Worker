using System.Collections.Generic;
using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlternativasLegadoPorIdQuery : IRequest<IEnumerable<long>>
    {
        public long QuestaoId { get; private set; }

        public ObterAlternativasLegadoPorIdQuery(long questaoId)
        {
            QuestaoId = questaoId;
        }
    }
}