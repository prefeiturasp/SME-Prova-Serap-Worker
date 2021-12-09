using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class InserirDresCommandHandler : IRequestHandler<InserirDresCommand, bool>
    {
        private readonly IRepositorioDreEntity repositorioDreEntity;

        public InserirDresCommandHandler(IRepositorioDreEntity repositorioDreEntity)
        {
            this.repositorioDreEntity = repositorioDreEntity ?? throw new System.ArgumentNullException(nameof(repositorioDreEntity));
        }

        public async Task<bool> Handle(InserirDresCommand request, CancellationToken cancellationToken)
        {
            await repositorioDreEntity.InserirVariosAsync(request.Dres);
            return true;
        }

    }
}
