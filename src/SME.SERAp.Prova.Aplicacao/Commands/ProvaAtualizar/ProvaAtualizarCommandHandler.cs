using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ProvaAtualizarCommandHandler : IRequestHandler<ProvaAtualizarCommand, long>
    {
        private readonly IRepositorioProva repositorioProva;

        public ProvaAtualizarCommandHandler(IRepositorioProva repositorioProva)
        {
            this.repositorioProva = repositorioProva ?? throw new System.ArgumentNullException(nameof(repositorioProva));
        }
        public async Task<long> Handle(ProvaAtualizarCommand request, CancellationToken cancellationToken)
        {
            return await repositorioProva.UpdateAsync(request.Prova);
        }
    }
}
