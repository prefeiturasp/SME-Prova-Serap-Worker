using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterContextosProvaLegadoPorProvaIdQueryHandler : IRequestHandler<ObterContextosProvaLegadoPorProvaIdQuery, IEnumerable<ContextoProvaLegadoDto>>
    {
        private readonly IRepositorioProvaLegado repositorioProvaLegado;

        public ObterContextosProvaLegadoPorProvaIdQueryHandler(IRepositorioProvaLegado repositorioProvaLegado)
        {
            this.repositorioProvaLegado = repositorioProvaLegado ?? throw new ArgumentNullException(nameof(repositorioProvaLegado));
        }
        public async Task<IEnumerable<ContextoProvaLegadoDto>> Handle(ObterContextosProvaLegadoPorProvaIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioProvaLegado.ObterContextosProvaPorProvaId(request.ProvaId);
        }
    }
}
