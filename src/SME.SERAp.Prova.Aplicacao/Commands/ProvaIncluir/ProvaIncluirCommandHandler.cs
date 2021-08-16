using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ProvaIncluirCommandHandler : IRequestHandler<ProvaIncluirCommand, long>
    {
        private readonly IRepositorioProva repositorioProva;

        public ProvaIncluirCommandHandler(IRepositorioProva repositorioProva)
        {
            this.repositorioProva = repositorioProva ?? throw new System.ArgumentNullException(nameof(repositorioProva));
        }
        public async Task<long> Handle(ProvaIncluirCommand request, CancellationToken cancellationToken)
        {
            return await repositorioProva.IncluirAsync(request.Prova);
        }
    }
}
