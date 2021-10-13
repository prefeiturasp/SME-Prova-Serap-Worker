using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUesSgpPorDreCodigoQueryHandler : IRequestHandler<ObterUesSgpPorDreCodigoQuery, IEnumerable<Ue>>
    {
        private readonly IRepositorioUe repositorioUe;

        public ObterUesSgpPorDreCodigoQueryHandler(IRepositorioUe repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<IEnumerable<Ue>> Handle(ObterUesSgpPorDreCodigoQuery request, CancellationToken cancellationToken)
            => await repositorioUe.ObterUesSgpPorDreCodigo(request.DreCodigo);
    }
}
