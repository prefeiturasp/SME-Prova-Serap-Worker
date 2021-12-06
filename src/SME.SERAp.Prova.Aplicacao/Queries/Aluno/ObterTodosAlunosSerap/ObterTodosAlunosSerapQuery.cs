using MediatR;
using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTodosAlunosSerapQuery : IRequest<IEnumerable<Aluno>>
    {
        public ObterTodosAlunosSerapQuery()
        {

        }
    }

}
