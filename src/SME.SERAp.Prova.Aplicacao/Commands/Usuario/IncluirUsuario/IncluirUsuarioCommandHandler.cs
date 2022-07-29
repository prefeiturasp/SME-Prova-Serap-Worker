using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class IncluirUsuarioCommandHandler : IRequestHandler<IncluirUsuarioCommand, long>
    {
        private readonly IRepositorioUsuario repositorioUsuario;

        public IncluirUsuarioCommandHandler(IRepositorioUsuario repositorioUsuario)
        {
            this.repositorioUsuario = repositorioUsuario ?? throw new System.ArgumentNullException(nameof(repositorioUsuario));
        }
        public async Task<long> Handle(IncluirUsuarioCommand request, CancellationToken cancellationToken)
            => await repositorioUsuario.SalvarAsync(request.Usuario);
    }
}
