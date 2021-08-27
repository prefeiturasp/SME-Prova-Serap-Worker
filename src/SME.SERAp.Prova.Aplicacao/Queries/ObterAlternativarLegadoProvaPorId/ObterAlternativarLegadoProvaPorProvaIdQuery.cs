using System.Collections.Generic;
using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlternativarLegadoProvaPorProvaIdQuery : IRequest<IEnumerable<long>>
    {
        public long QuestaoId { get; private set; }

        public ObterAlternativarLegadoProvaPorProvaIdQuery(long questaoId)
        {
            QuestaoId = questaoId;
        }
    }
}