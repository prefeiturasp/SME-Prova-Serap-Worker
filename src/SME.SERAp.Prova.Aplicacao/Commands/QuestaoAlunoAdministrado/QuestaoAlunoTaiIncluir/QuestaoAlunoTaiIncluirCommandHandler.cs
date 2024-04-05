using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;

namespace SME.SERAp.Prova.Aplicacao
{
    public class QuestaoAlunoTaiIncluirCommandHandler : IRequestHandler<QuestaoAlunoTaiIncluirCommand, long>
    {
        private readonly IRepositorioQuestaoAlunoTai repositorioQuestaoAlunoTai;

        public QuestaoAlunoTaiIncluirCommandHandler(IRepositorioQuestaoAlunoTai repositorioQuestaoAlunoTai)
        {
            this.repositorioQuestaoAlunoTai = repositorioQuestaoAlunoTai ?? throw new ArgumentNullException(nameof(repositorioQuestaoAlunoTai));
        }

        public async Task<long> Handle(QuestaoAlunoTaiIncluirCommand request, CancellationToken cancellationToken)
        {
            return await repositorioQuestaoAlunoTai.IncluirAsync(request.QuestaoAlunoTai);
        }
    }
}