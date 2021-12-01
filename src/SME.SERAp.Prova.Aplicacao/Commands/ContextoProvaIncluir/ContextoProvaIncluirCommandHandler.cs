using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ContextoProvaIncluirCommandHandler : IRequestHandler<ContextoProvaIncluirCommand, long>
    {
        private readonly IRepositorioContextoProva repositorioContextoProva;

        public ContextoProvaIncluirCommandHandler(IRepositorioContextoProva repositorioContextoProva)
        {
            this.repositorioContextoProva = repositorioContextoProva ?? throw new System.ArgumentNullException(nameof(repositorioContextoProva));
        }
        public async Task<long> Handle(ContextoProvaIncluirCommand request, CancellationToken cancellationToken)
        {
            return await repositorioContextoProva.IncluirAsync(request.ContextoProva);
        }
    }
}
