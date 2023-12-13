using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterQuestaoCompletaPorQuestaoIdQueryHandler : IRequestHandler<ObterQuestaoCompletaPorQuestaoIdQuery, QuestaoCompleta>
    {
        private readonly IRepositorioQuestaoCompleta repositorioQuestaoCompleta;

        public ObterQuestaoCompletaPorQuestaoIdQueryHandler(IRepositorioQuestaoCompleta repositorioQuestaoCompleta)
        {
            this.repositorioQuestaoCompleta = repositorioQuestaoCompleta ?? throw new ArgumentNullException(nameof(repositorioQuestaoCompleta));
        }

        public async Task<QuestaoCompleta> Handle(ObterQuestaoCompletaPorQuestaoIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioQuestaoCompleta.ObterQuestaoCompletaPorQuestaoIdAsync(request.QuestaoId);
        }
    }
}