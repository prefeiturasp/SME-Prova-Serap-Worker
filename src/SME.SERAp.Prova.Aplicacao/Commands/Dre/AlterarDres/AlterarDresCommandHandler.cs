using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AlterarDresCommandHandler : IRequestHandler<AlterarDresCommand, bool>
    {
        private readonly IRepositorioDreEntity repositorioDreEntity;

        public AlterarDresCommandHandler(IRepositorioDreEntity repositorioDreEntity)
        {
            this.repositorioDreEntity = repositorioDreEntity ?? throw new System.ArgumentNullException(nameof(repositorioDreEntity));
        }

        public async Task<bool> Handle(AlterarDresCommand request, CancellationToken cancellationToken)
        {
            await repositorioDreEntity.AlterarVariosAsync(request.Dres);
            return true;
        }

    }
}
