using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTipoProvaPorIdQueryHandler : IRequestHandler<ObterTipoProvaPorIdQuery, TipoProva>
    {
        private readonly IRepositorioTipoProva repositorioTipoProva;

        public ObterTipoProvaPorIdQueryHandler(IRepositorioTipoProva repositorioTipoProva)
        {
            this.repositorioTipoProva = repositorioTipoProva ?? throw new ArgumentNullException(nameof(repositorioTipoProva));
        }

        public async Task<TipoProva> Handle(ObterTipoProvaPorIdQuery request, CancellationToken cancellationToken)
                => await repositorioTipoProva.ObterPorIdAsync(request.Id);
    }
}
