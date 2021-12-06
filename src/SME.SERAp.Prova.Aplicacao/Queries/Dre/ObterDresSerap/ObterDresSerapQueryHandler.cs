using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterDresSerapQueryHandler : IRequestHandler<ObterDresSerapQuery, IEnumerable<Dre>>
    {
        private readonly IRepositorioDre repositorioDre;

        public ObterDresSerapQueryHandler(IRepositorioDre repositorioDre)
        {
            this.repositorioDre = repositorioDre ?? throw new System.ArgumentNullException(nameof(repositorioDre));
        }
        public async Task<IEnumerable<Dre>> Handle(ObterDresSerapQuery request, CancellationToken cancellationToken)
        {
            return await repositorioDre.ObterTudoAsync();
        }
    }
}
