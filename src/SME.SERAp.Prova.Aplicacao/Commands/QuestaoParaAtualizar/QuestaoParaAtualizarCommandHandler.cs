using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class QuestaoParaAtualizarCommandHandler : IRequestHandler<QuestaoParaAtualizarCommand, long>
    {
        private readonly IRepositorioQuestao repositorioQuestao;

        public QuestaoParaAtualizarCommandHandler(IRepositorioQuestao repositorioQuestao)
        {
            this.repositorioQuestao = repositorioQuestao ?? throw new ArgumentNullException(nameof(repositorioQuestao));
        }

        public async Task<long> Handle(QuestaoParaAtualizarCommand request, CancellationToken cancellationToken)
        {
            return await repositorioQuestao.UpdateAsync(request.Questao);
        }
    }
}