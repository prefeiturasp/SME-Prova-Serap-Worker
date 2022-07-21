using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AlterarUsuarioCommandHandler : IRequestHandler<AlterarUsuarioCommand, bool>
    {
        private readonly IRepositorioUsuario repositorioUsuario;

        public AlterarUsuarioCommandHandler(IRepositorioUsuario repositorioUsuario)
        {
            this.repositorioUsuario =
                repositorioUsuario ?? throw new System.ArgumentNullException(nameof(repositorioUsuario));
        }

        public async Task<bool> Handle(AlterarUsuarioCommand request, CancellationToken cancellationToken)
            => await repositorioUsuario.SalvarAsync(request.Usuario) > 0;
    }
}
