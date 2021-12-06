using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class InserirUesCommandHandler : IRequestHandler<InserirUesCommand, bool>
    {
        private readonly IRepositorioUeEntity repositorioUe;

        public InserirUesCommandHandler(IRepositorioUeEntity repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new System.ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<bool> Handle(InserirUesCommand request, CancellationToken cancellationToken)
        {
            await repositorioUe.InserirVariosAsync(request.Ues);
            return true;
        }

    }
}
