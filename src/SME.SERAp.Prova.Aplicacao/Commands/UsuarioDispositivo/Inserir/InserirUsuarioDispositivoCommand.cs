using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class InserirUsuarioDispositivoCommand : IRequest<bool>
    {
        public InserirUsuarioDispositivoCommand(UsuarioDispositivo usuarioDispositivo)
        {
            UsuarioDispositivo = usuarioDispositivo;
        }

        public UsuarioDispositivo UsuarioDispositivo { get; set; }

    }
}
