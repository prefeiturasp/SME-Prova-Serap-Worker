using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterProvasLiberadasNoPeriodoParaCacheQueryHandler : IRequestHandler<ObterProvasLiberadasNoPeriodoParaCacheQuery, IEnumerable<Dominio.Prova>>
    {
        private readonly IRepositorioProva repositorioProva;

        public ObterProvasLiberadasNoPeriodoParaCacheQueryHandler(IRepositorioProva repositorioProva)
        {
            this.repositorioProva = repositorioProva ?? throw new ArgumentNullException(nameof(repositorioProva));
        }

        public async Task<IEnumerable<Dominio.Prova>> Handle(ObterProvasLiberadasNoPeriodoParaCacheQuery request, CancellationToken cancellationToken)
        {
            return await repositorioProva.ObterProvasLiberadasNoPeriodoParaCacheAsync();
        }
    }
}