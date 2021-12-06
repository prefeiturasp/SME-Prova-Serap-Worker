using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUesSerapPorDreCodigoQueryHandler : IRequestHandler<ObterUesSerapPorDreCodigoQuery, IEnumerable<Ue>>
    {
        private readonly IRepositorioUe repositorioUe;

        public ObterUesSerapPorDreCodigoQueryHandler(IRepositorioUe repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<IEnumerable<Ue>> Handle(ObterUesSerapPorDreCodigoQuery request, CancellationToken cancellationToken)
            => await repositorioUe.ObterUesSerapPorDreCodigoAsync(request.DreCodigo);
    }
}
