using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AlterarUsuarioSerapCoreSsoCommand : IRequest<bool>
    {
        public AlterarUsuarioSerapCoreSsoCommand(UsuarioSerapCoreSso usuario)
        {
            Usuario = usuario;
        }

        public UsuarioSerapCoreSso Usuario { get; set; }
    }
}
