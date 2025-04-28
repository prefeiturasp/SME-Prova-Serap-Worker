using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.Commands
{
    public class IncluirQuestaoAlunoTaiCommandHandler : IRequestHandler<IncluirQuestaoAlunoTaiCommand, long>
    {
        private readonly IRepositorioQuestaoAlunoTai repositorioQuestaoAlunoTai;
        public IncluirQuestaoAlunoTaiCommandHandler(IRepositorioQuestaoAlunoTai repositorioQuestaoAlunoTai)
        {
            this.repositorioQuestaoAlunoTai = repositorioQuestaoAlunoTai;
        }

        public Task<long> Handle(IncluirQuestaoAlunoTaiCommand request, CancellationToken cancellationToken)
        {
            return this.repositorioQuestaoAlunoTai.IncluirAsync(request.QuestaoAlunoTai);
        }
    }
}
