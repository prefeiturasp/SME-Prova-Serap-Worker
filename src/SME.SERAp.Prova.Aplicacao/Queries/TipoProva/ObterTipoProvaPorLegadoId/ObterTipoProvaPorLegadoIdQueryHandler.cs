using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTipoProvaPorLegadoIdQueryHandler :
        IRequestHandler<ObterTipoProvaPorLegadoIdQuery, TipoProva>
    {
        private readonly IRepositorioTipoProva repositorioTipoProva;
        public ObterTipoProvaPorLegadoIdQueryHandler(IRepositorioTipoProva repositorioTipoProva)
        {
            this.repositorioTipoProva = repositorioTipoProva ?? throw new ArgumentNullException(nameof(repositorioTipoProva));
        }

        public async Task<TipoProva> Handle(ObterTipoProvaPorLegadoIdQuery request,
            CancellationToken cancellationToken)
            => await repositorioTipoProva.ObterPorLegadoId(request.LegadoId);
    }
}
