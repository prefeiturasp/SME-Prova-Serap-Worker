using MediatR;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmaAtribuidasEolPorUsuarioQuery : IRequest<IEnumerable<TurmaAtribuicaoEolDto>>
    {
        public ObterTurmaAtribuidasEolPorUsuarioQuery(string login)
        {
            Login = login;
        }

        public string Login { get; set; }
    }
}
