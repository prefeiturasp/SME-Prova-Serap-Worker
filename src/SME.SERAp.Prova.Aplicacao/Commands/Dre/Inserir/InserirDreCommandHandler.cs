using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class InserirDreCommandHandler : IRequestHandler<InserirDreCommand, bool>
    {
        private readonly IRepositorioDre repositorioDre;

        public InserirDreCommandHandler(IRepositorioDre repositorioDre)
        {
            this.repositorioDre = repositorioDre ?? throw new ArgumentNullException(nameof(repositorioDre));
        }

        public async Task<bool> Handle(InserirDreCommand request, CancellationToken cancellationToken)
            => await repositorioDre.SalvarAsync(request.Dre) != 0;
    }
}
