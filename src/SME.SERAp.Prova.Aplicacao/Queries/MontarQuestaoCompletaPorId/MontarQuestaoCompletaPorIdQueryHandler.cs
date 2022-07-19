using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class MontarQuestaoCompletaPorIdQueryHandler : IRequestHandler<MontarQuestaoCompletaPorIdQuery, QuestaoCompletaDto>
    {

        private readonly IRepositorioQuestao repositorioQuestao;

        public MontarQuestaoCompletaPorIdQueryHandler(IRepositorioQuestao repositorioQuestao)
        {
            this.repositorioQuestao = repositorioQuestao ?? throw new ArgumentException(nameof(repositorioQuestao));
        }

        public async Task<QuestaoCompletaDto> Handle(MontarQuestaoCompletaPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioQuestao.MontarQuestaoCompletaPorIdAsync(request.QuestaoId);
        }
    }
}
