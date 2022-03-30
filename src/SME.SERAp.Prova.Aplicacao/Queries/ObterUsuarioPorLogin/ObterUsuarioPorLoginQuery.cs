using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUsuarioPorLoginQuery : IRequest<Usuario>
    {
        public ObterUsuarioPorLoginQuery(long login)
        {
            Login = login;
        }

        public long Login { get; set; }
    }
}
