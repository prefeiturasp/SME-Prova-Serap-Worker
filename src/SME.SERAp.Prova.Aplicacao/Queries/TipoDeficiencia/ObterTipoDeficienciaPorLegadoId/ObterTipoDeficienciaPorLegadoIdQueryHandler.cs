using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTipoDeficienciaPorLegadoIdQueryHandler : IRequestHandler<ObterTipoDeficienciaPorLegadoIdQuery, TipoDeficiencia>
    {
        private readonly IRepositorioTipoDeficiencia repositorioTipoDeficiencia;

        public ObterTipoDeficienciaPorLegadoIdQueryHandler(IRepositorioTipoDeficiencia repositorioTipoDeficiencia)
        {
            this.repositorioTipoDeficiencia = repositorioTipoDeficiencia ?? throw new ArgumentNullException(nameof(repositorioTipoDeficiencia));
        }

        public async Task<TipoDeficiencia> Handle(ObterTipoDeficienciaPorLegadoIdQuery request,
            CancellationToken cancellationToken)
            => await repositorioTipoDeficiencia.ObterPorLegadoId(request.LegadoId);
    }
}
