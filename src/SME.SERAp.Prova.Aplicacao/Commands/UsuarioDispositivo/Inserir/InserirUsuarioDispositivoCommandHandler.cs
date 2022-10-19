using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class InserirUsuarioDispositivoCommandHandler : IRequestHandler<InserirUsuarioDispositivoCommand, bool>
    {

        private readonly IRepositorioUsuarioDispositivo repositorioUsuarioDispositivo;

        public InserirUsuarioDispositivoCommandHandler(IRepositorioUsuarioDispositivo repositorioUsuarioDispositivo)
        {
            this.repositorioUsuarioDispositivo = repositorioUsuarioDispositivo ?? throw new ArgumentNullException(nameof(repositorioUsuarioDispositivo));
        }

        public async Task<bool> Handle(InserirUsuarioDispositivoCommand request, CancellationToken cancellationToken)
        {
            await repositorioUsuarioDispositivo.SalvarAsync(request.UsuarioDispositivo);
            return true;
        }
    }
}
