using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TipoProvaAtualizarCommandHandler : IRequestHandler<TipoProvaAtualizarCommand, bool>
    {

        private readonly IRepositorioTipoProva repositorioTipoProva;

        public TipoProvaAtualizarCommandHandler(IRepositorioTipoProva repositorioTipoProva)
        {
            this.repositorioTipoProva = repositorioTipoProva ?? throw new System.ArgumentNullException(nameof(repositorioTipoProva));
        }

        public async Task<bool> Handle(TipoProvaAtualizarCommand request, CancellationToken cancellationToken)
        {
            await repositorioTipoProva.UpdateAsync(request.TipoProva);
            return true;
        }
    }
}
