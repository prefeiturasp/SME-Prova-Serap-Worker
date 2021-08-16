using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterProvaDetalhesPorIdQueryHandler : IRequestHandler<ObterProvaDetalhesPorIdQuery, Dominio.Prova>
    {
        private readonly IRepositorioProva repositorioProva;

        public ObterProvaDetalhesPorIdQueryHandler(IRepositorioProva repositorioProva)
        {
            this.repositorioProva = repositorioProva ?? throw new ArgumentNullException(nameof(repositorioProva));
        }
        public async Task<Dominio.Prova> Handle(ObterProvaDetalhesPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioProva.ObterPorIdLegadoAsync(request.ProvaId);
        }
    }
}
