using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class CriarProvaRespostasExtracaoCommandHandler : IRequestHandler<CriarProvaRespostasExtracaoCommand, bool>
    {
        private readonly IRepositorioProva repositorioProva;

        public CriarProvaRespostasExtracaoCommandHandler(IRepositorioProva repositorioProva)
        {
            this.repositorioProva = repositorioProva ?? throw new System.ArgumentNullException(nameof(repositorioProva));
        }
        public async Task<bool> Handle(CriarProvaRespostasExtracaoCommand request, CancellationToken cancellationToken)
        {
            await repositorioProva.CriarProvaRespostasExtracao(request.ProvaId);
            return true;
        }
    }
}
