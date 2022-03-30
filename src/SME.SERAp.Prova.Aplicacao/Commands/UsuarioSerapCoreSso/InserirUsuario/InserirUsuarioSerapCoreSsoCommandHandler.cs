using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class InserirUsuarioSerapCoreSsoCommandHandler : IRequestHandler<InserirUsuarioSerapCoreSsoCommand, long>
    {
        
        private readonly IRepositorioUsuarioSerapCoreSso repositorioUsuarioSerapCoreSso;

        public InserirUsuarioSerapCoreSsoCommandHandler(IRepositorioUsuarioSerapCoreSso repositorioUsuarioSerapCoreSso)
        {
            this.repositorioUsuarioSerapCoreSso = repositorioUsuarioSerapCoreSso ?? throw new System.ArgumentNullException(nameof(repositorioUsuarioSerapCoreSso));
        }

        public async Task<long> Handle(InserirUsuarioSerapCoreSsoCommand request, CancellationToken cancellationToken)
        {
            return await repositorioUsuarioSerapCoreSso.IncluirAsync(request.Usuario);
        }

    }
}
