using System.Collections.Generic;
using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlternativarProvaPorProvaIdQuery : IRequest<IEnumerable<AlternativasProvaIdDto>>
    {
        public long ProvaId { get; private set; }

        public ObterAlternativarProvaPorProvaIdQuery(long provaId)
        {
            ProvaId = provaId;
        }
    }
}