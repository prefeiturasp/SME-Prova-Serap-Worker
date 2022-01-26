using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTipoProvaDeficienciaPorTipoProvaLegadoIdQueryHandler : IRequestHandler<ObterTipoProvaDeficienciaPorTipoProvaLegadoIdQuery, IEnumerable<Guid>>
    {
        private readonly IRepositorioGeralSerapLegado repositorioGeralSerapLegado;

        public ObterTipoProvaDeficienciaPorTipoProvaLegadoIdQueryHandler(IRepositorioGeralSerapLegado repositorioGeralSerapLegado)
        {
            this.repositorioGeralSerapLegado = repositorioGeralSerapLegado ?? throw new ArgumentNullException(nameof(repositorioGeralSerapLegado));
        }

        public async Task<IEnumerable<Guid>> Handle(ObterTipoProvaDeficienciaPorTipoProvaLegadoIdQuery request, CancellationToken cancellationToken)
            => await repositorioGeralSerapLegado.ObterTipoProvaDeficienciaPorTipoProvaLegadoId(request.TipoProvaLegadoId);
    }
}
