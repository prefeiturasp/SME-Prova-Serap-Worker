using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUePorCodigoQueryHandler : IRequestHandler<ObterUePorCodigoQuery, Ue>
    {
        private readonly IRepositorioUe repositorioUe;

        public ObterUePorCodigoQueryHandler(IRepositorioUe repositorioUe)
        {
            this.repositorioUe = repositorioUe ?? throw new ArgumentNullException(nameof(repositorioUe));
        }

        public async Task<Ue> Handle(ObterUePorCodigoQuery request, CancellationToken cancellationToken)
            => await repositorioUe.ObterUePorCodigo(request.UeCodigo);
    }
}
