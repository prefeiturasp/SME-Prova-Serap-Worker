using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ConsolidarProvaRespostaPorProvaSerapIdCommandHandler : IRequestHandler<ConsolidarProvaRespostaPorProvaSerapIdCommand, bool>
    {
        private readonly IRepositorioProva repositorioProva;

        public ConsolidarProvaRespostaPorProvaSerapIdCommandHandler(IRepositorioProva repositorioProva)
        {
            this.repositorioProva = repositorioProva ?? throw new System.ArgumentNullException(nameof(repositorioProva));
        }
        public async Task<bool> Handle(ConsolidarProvaRespostaPorProvaSerapIdCommand request, CancellationToken cancellationToken)
        {
            await repositorioProva.ConsolidarProvaRespostasPorProvaSerapId(request.ProvaId);
            return true;
        }
    }
}
