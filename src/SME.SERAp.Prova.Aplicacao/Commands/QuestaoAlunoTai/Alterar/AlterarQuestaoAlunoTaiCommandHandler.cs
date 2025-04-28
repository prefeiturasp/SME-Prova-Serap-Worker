using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.Commands
{
    public class AlterarQuestaoAlunoTaiCommandHandler : IRequestHandler<AlterarQuestaoAlunoTaiCommand, long>
    {
        private readonly IRepositorioQuestaoAlunoTai repositorioQuestaoAlunoTai;

        public AlterarQuestaoAlunoTaiCommandHandler(IRepositorioQuestaoAlunoTai repositorioQuestaoAlunoTai)
        {
            this.repositorioQuestaoAlunoTai = repositorioQuestaoAlunoTai;
        }

        public Task<long> Handle(AlterarQuestaoAlunoTaiCommand request, CancellationToken cancellationToken)
        {
            return this.repositorioQuestaoAlunoTai.UpdateAsync(request.QuestaoAlunoTai);
        }
    }
}
