using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterDetalheQuestoesPorProvaIdQueryHandler :
        IRequestHandler<ObterDetalheQuestoesPorProvaIdQuery, QuestoesPorProvaIdDto>
    {
        private readonly IRepositorioProvaLegado repositorioProvaLegado;

        public ObterDetalheQuestoesPorProvaIdQueryHandler(IRepositorioProvaLegado repositorioProvaLegado)
        {
            this.repositorioProvaLegado = repositorioProvaLegado ??
                                          throw new ArgumentNullException(nameof(repositorioProvaLegado));
        }

        public async Task<QuestoesPorProvaIdDto> Handle(ObterDetalheQuestoesPorProvaIdQuery request,
            CancellationToken cancellationToken)
            => await repositorioProvaLegado.ObterDetalheQuestoesPorProvaId(request.ProvaLegadoId, request.QuestaoLegadoId);
    }
}