using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTipoDeficienciaPorCodigoEolQueryHandler : IRequestHandler<ObterTipoDeficienciaPorCodigoEolQuery, TipoDeficiencia>
    {

        private readonly IRepositorioTipoDeficiencia repositorioTipoDeficiencia;

        public ObterTipoDeficienciaPorCodigoEolQueryHandler(IRepositorioTipoDeficiencia repositorioTipoDeficiencia)
        {
            this.repositorioTipoDeficiencia = repositorioTipoDeficiencia ?? throw new ArgumentNullException(nameof(repositorioTipoDeficiencia));
        }

        public async Task<TipoDeficiencia> Handle(ObterTipoDeficienciaPorCodigoEolQuery request,
            CancellationToken cancellationToken)
            => await repositorioTipoDeficiencia.ObterPorCodigoEol(request.CodigoEol);
    }
}
