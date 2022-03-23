using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterDrePorIdQueryHandler : IRequestHandler<ObterDrePorIdQuery, Dre>
    {

        private readonly IRepositorioDre repositorioDre;

        public ObterDrePorIdQueryHandler(IRepositorioDre repositorioDre)
        {
            this.repositorioDre = repositorioDre ?? throw new ArgumentNullException(nameof(repositorioDre));
        }

        public async Task<Dre> Handle(ObterDrePorIdQuery request, CancellationToken cancellationToken)
            => await repositorioDre.ObterPorIdAsync(request.DreId);

    }
}
