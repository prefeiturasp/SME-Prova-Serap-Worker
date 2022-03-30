using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AlterarUsuarioSerapCoreSsoCommandHandler : IRequestHandler<AlterarUsuarioSerapCoreSsoCommand, bool>
    {
        
        private readonly IRepositorioUsuarioSerapCoreSso repositorioUsuarioSerapCoreSso;

        public AlterarUsuarioSerapCoreSsoCommandHandler(IRepositorioUsuarioSerapCoreSso repositorioUsuarioSerapCoreSso)
        {
            this.repositorioUsuarioSerapCoreSso = repositorioUsuarioSerapCoreSso ?? throw new System.ArgumentNullException(nameof(repositorioUsuarioSerapCoreSso));
        }

        public async Task<bool> Handle(AlterarUsuarioSerapCoreSsoCommand request, CancellationToken cancellationToken)
        {
            return await repositorioUsuarioSerapCoreSso.UpdateAsync(request.Usuario) > 0;
        }
    }
}
