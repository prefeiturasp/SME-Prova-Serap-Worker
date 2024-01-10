using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterQuestoesPorProvaIdQueryHandler :
        IRequestHandler<ObterQuestoesPorProvaIdQuery, IEnumerable<QuestoesPorProvaIdDto>>
    {
        private readonly IRepositorioProvaLegado repositorioProvaLegado;

        public ObterQuestoesPorProvaIdQueryHandler(IRepositorioProvaLegado repositorioProvaLegado)
        {
            this.repositorioProvaLegado = repositorioProvaLegado ??
                                          throw new ArgumentNullException(nameof(repositorioProvaLegado));
        }

        public async Task<IEnumerable<QuestoesPorProvaIdDto>> Handle(ObterQuestoesPorProvaIdQuery request,
            CancellationToken cancellationToken)
            => await repositorioProvaLegado.ObterQuestoesPorProvaId(request.ProvaId);
    }
}