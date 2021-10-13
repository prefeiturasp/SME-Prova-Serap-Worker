using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterQuestaoDetalheLegadoPorIdQueryHandler :
        IRequestHandler<ObterQuestaoDetalheLegadoPorIdQuery, QuestoesPorProvaIdDto>
    {
        private readonly IRepositorioProvaLegado repositorioProvaLegado;

        public ObterQuestaoDetalheLegadoPorIdQueryHandler(IRepositorioProvaLegado repositorioProvaLegado)
        {
            this.repositorioProvaLegado = repositorioProvaLegado ??
                                          throw new ArgumentNullException(nameof(repositorioProvaLegado));
        }

        public async Task<QuestoesPorProvaIdDto> Handle(ObterQuestaoDetalheLegadoPorIdQuery request,
            CancellationToken cancellationToken)
            => await repositorioProvaLegado.ObterDetalheQuestoesPorProvaId(request.ProvaLegadoId, request.QuestaoLegadoId);
    }
}