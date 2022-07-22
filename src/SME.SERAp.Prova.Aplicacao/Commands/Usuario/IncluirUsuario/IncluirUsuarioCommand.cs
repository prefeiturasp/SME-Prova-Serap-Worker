using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class IncluirUsuarioCommand : IRequest<long>
    {
        public IncluirUsuarioCommand(Usuario usuario)
        {
            Usuario = usuario;
        }
        public Usuario Usuario { get; set; }
    }
}
