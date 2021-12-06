using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AlterarUesCommandHandler : IRequestHandler<AlterarUesCommand, bool>
    {
        private readonly IRepositorioUeEntity repositorioUeEntity;

        public AlterarUesCommandHandler(IRepositorioUeEntity repositorioUeEntity)
        {
            this.repositorioUeEntity = repositorioUeEntity ?? throw new System.ArgumentNullException(nameof(repositorioUeEntity));
        }

        public async Task<bool> Handle(AlterarUesCommand request, CancellationToken cancellationToken)
        {
            await repositorioUeEntity.AlterarVariosAsync(request.Ues);
            return true;
        }

    }
}
