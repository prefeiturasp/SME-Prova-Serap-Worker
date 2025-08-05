using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.Commands
{
    public class ExcluirQuestaoAlunoTaiCommandHandler : IRequestHandler<ExcluirQuestaoAlunoTaiCommand, int>
    {
        private readonly IRepositorioQuestaoAlunoTai repositorioQuestaoAlunoTai;
        public ExcluirQuestaoAlunoTaiCommandHandler(IRepositorioQuestaoAlunoTai repositorioQuestaoAlunoTai)
        {
            this.repositorioQuestaoAlunoTai = repositorioQuestaoAlunoTai;
        }
        public async Task<int> Handle(ExcluirQuestaoAlunoTaiCommand request, CancellationToken cancellationToken)
        {
            return await repositorioQuestaoAlunoTai.ExcluirQuestaoAlunoTai(request.ProvaId, request.AlunoRa);
        }
    }
}
