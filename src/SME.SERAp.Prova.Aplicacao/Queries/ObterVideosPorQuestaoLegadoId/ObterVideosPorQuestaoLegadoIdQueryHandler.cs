using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterVideosPorQuestaoLegadoIdQueryHandler : IRequestHandler<ObterVideosPorQuestaoLegadoIdQuery, IEnumerable<QuestaoVideoDto>>
    {

        private readonly IRepositorioProvaLegado repositorioProvaLegado;
        public ObterVideosPorQuestaoLegadoIdQueryHandler(IRepositorioProvaLegado repositorioProvaLegado)
        {
            this.repositorioProvaLegado = repositorioProvaLegado ??
                                          throw new ArgumentNullException(nameof(repositorioProvaLegado));
        }

        public async Task<IEnumerable<QuestaoVideoDto>> Handle(ObterVideosPorQuestaoLegadoIdQuery request,
            CancellationToken cancellationToken)
            => await repositorioProvaLegado.ObterVideosPorQuestaoId(request.QuestaoId);
    }
}
