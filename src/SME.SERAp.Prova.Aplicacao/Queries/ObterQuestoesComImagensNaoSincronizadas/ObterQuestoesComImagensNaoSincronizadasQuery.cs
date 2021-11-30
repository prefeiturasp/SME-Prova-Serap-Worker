using MediatR;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterQuestoesComImagensNaoSincronizadasQuery : IRequest<IEnumerable<Dominio.Questao>>
    {
        public ObterQuestoesComImagensNaoSincronizadasQuery()
        {
        }
    }
}
