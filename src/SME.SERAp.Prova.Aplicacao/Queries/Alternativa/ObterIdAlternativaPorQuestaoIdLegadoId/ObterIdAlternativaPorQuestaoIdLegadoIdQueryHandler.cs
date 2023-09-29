using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterIdAlternativaPorQuestaoIdLegadoIdQueryHandler : IRequestHandler<ObterIdAlternativaPorQuestaoIdLegadoIdQuery, long>
    {
        private readonly IRepositorioAlternativa repositorioAlternativa;

        public ObterIdAlternativaPorQuestaoIdLegadoIdQueryHandler(IRepositorioAlternativa repositorioAlternativa)
        {
            this.repositorioAlternativa = repositorioAlternativa ?? throw new ArgumentNullException(nameof(repositorioAlternativa));
        }

        public async Task<long> Handle(ObterIdAlternativaPorQuestaoIdLegadoIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAlternativa.ObterIdAlternativaPorQuestaoIdLegadoId(request.QuestaoId, request.AlternativaLegadoId);
        }
    }
}
