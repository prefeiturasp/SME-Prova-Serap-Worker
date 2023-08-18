using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlternativasLegadoPorIdQueryHandler :
        IRequestHandler<ObterAlternativasLegadoPorIdQuery, IEnumerable<long>>
    {
        private readonly IRepositorioProvaLegado repositorioProvaLegado;

        public ObterAlternativasLegadoPorIdQueryHandler(IRepositorioProvaLegado repositorioProvaLegado)
        {
            this.repositorioProvaLegado = repositorioProvaLegado ??
                                          throw new ArgumentNullException(nameof(repositorioProvaLegado));
        }

        public async Task<IEnumerable<long>> Handle(ObterAlternativasLegadoPorIdQuery request,
            CancellationToken cancellationToken)
            => await repositorioProvaLegado.ObterAlternativasPorProvaIdEQuestaoId(request.QuestaoId);
    }
}