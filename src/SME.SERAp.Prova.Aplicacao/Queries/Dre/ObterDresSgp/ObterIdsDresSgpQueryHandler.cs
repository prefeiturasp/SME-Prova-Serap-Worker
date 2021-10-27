using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterIdsDresSgpQueryHandler : IRequestHandler<ObterDresSgpQuery, IEnumerable<Dre>>
    {
        private readonly IRepositorioDre repositorioDre;

        public ObterIdsDresSgpQueryHandler(IRepositorioDre repositorioDre)
        {
            this.repositorioDre = repositorioDre ?? throw new ArgumentNullException(nameof(repositorioDre));
        }

        public async Task<IEnumerable<Dre>> Handle(ObterDresSgpQuery request, CancellationToken cancellationToken)
            => await repositorioDre.ObterDresSgp();
    }
}
