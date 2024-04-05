using System.Collections.Generic;
using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterResumoQuestoesPorProvaIdParaCacheQuery : IRequest<IEnumerable<ResumoQuestaoProvaDto>>
    {
        public ObterResumoQuestoesPorProvaIdParaCacheQuery(long provaId)
        {
            ProvaId = provaId;
        }

        public long ProvaId { get; }
    }
}