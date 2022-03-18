using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class InserirUsuarioSerapCoreSsoCommand : IRequest<long>
    {
        public InserirUsuarioSerapCoreSsoCommand(UsuarioSerapCoreSso usuario)
        {
            Usuario = usuario;
        }

        public UsuarioSerapCoreSso Usuario { get; set; }

    }
}
