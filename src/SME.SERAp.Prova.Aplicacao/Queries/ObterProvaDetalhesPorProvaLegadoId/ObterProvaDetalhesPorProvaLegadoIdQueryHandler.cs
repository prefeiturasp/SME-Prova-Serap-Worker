using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterProvaDetalhesPorProvaLegadoIdQueryHandler : IRequestHandler<ObterProvaDetalhesPorProvaLegadoIdQuery, Dominio.Prova>
    {
        private readonly IRepositorioProva repositorioProva;

        public ObterProvaDetalhesPorProvaLegadoIdQueryHandler(IRepositorioProva repositorioProva)
        {
            this.repositorioProva = repositorioProva ?? throw new ArgumentNullException(nameof(repositorioProva));
        }

        public async Task<Dominio.Prova> Handle(ObterProvaDetalhesPorProvaLegadoIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioProva.ObterPorIdLegadoAsync(request.ProvaLegadoId);
        }
    }
}
