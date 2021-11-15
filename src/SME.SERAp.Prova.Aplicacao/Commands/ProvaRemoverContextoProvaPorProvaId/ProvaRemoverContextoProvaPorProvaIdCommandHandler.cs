using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ProvaRemoverContextoProvaPorProvaIdCommandHandler : IRequestHandler<ProvaRemoverContextoProvaPorProvaIdCommand, bool>
    {
        private readonly IRepositorioContextoProva repositorioContextoProva;

        public ProvaRemoverContextoProvaPorProvaIdCommandHandler(IRepositorioContextoProva repositorioContextoProva)
        {
            this.repositorioContextoProva = repositorioContextoProva ?? throw new ArgumentNullException(nameof(repositorioContextoProva));
        }
        public async Task<bool> Handle(ProvaRemoverContextoProvaPorProvaIdCommand request, CancellationToken cancellationToken)
        {
            return await repositorioContextoProva.RemoverContextosProvaPorProvaIdAsync(request.Id);
        }
    }
}
