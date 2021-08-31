using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlternativaDetalheLegadoPorIdQueryHandler :
        IRequestHandler<ObterAlternativaDetalheLegadoPorIdQuery, AlternativasProvaIdDto>
    {
        private readonly IRepositorioProvaLegado repositorioProvaLegado;

        public ObterAlternativaDetalheLegadoPorIdQueryHandler(IRepositorioProvaLegado repositorioProvaLegado)
        {
            this.repositorioProvaLegado = repositorioProvaLegado ??
                                          throw new ArgumentNullException(nameof(repositorioProvaLegado));
        }

        public async Task<AlternativasProvaIdDto> Handle(ObterAlternativaDetalheLegadoPorIdQuery request,
            CancellationToken cancellationToken)
            => await repositorioProvaLegado.ObterDetalheAlternativasPorProvaIdEQuestaoId(request.QuestaoId, request.AlternativaId);
    }
}