using MediatR;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlternativasComImagensNaoSincronizadasQuery : IRequest<IEnumerable<Dominio.Alternativa>>
    {
        public ObterAlternativasComImagensNaoSincronizadasQuery()
        {
        }
    }
}
