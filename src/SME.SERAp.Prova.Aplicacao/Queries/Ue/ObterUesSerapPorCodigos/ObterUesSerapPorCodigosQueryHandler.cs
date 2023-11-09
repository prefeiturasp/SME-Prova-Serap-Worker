using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUesSerapPorCodigosQueryHandler : IRequestHandler<ObterUesSerapPorCodigosQuery, IEnumerable<Ue>>
    {
        private readonly IRepositorioUe repositorioUe;

        public ObterUesSerapPorCodigosQueryHandler(IRepositorioUe repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<IEnumerable<Ue>> Handle(ObterUesSerapPorCodigosQuery request, CancellationToken cancellationToken)
        {
            return await repositorioUe.ObterUesPorCodigosAsync(request.Codigos);
        }
    }
}
