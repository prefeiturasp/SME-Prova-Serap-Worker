using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AlterarUsuarioCommand : IRequest<bool>
    {
        public AlterarUsuarioCommand(Usuario usuario)
        {
            Usuario = usuario;
        }

        public Usuario Usuario { get; set; }
    }
 
}
