using System.Collections.Generic;
using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlternativarLegadoProvaPorProvaIdQuery : IRequest<IEnumerable<long>>
    {
        public long ProvaId { get; private set; }
        public long QuestaoId { get; private set; }

        public ObterAlternativarLegadoProvaPorProvaIdQuery(long provaId, long questaoId)
        {
            ProvaId = provaId;
            QuestaoId = questaoId;
        }
    }
}