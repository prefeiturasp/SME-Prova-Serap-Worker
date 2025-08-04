using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.Commands
{
    public class ExcluirRespostaAlunoCommandHandler : IRequestHandler<ExcluirRespostaAlunoCommand, int>
    {
        private readonly IRepositorioQuestaoAlunoResposta repositorioQuestaoAlunoResposta;
        public ExcluirRespostaAlunoCommandHandler(IRepositorioQuestaoAlunoResposta repositorioQuestaoAlunoResposta)
        {
            this.repositorioQuestaoAlunoResposta = repositorioQuestaoAlunoResposta;
        }

        public Task<int> Handle(ExcluirRespostaAlunoCommand request, CancellationToken cancellationToken)
        {
            return repositorioQuestaoAlunoResposta.ExcluirRespostaAluno(request.ProvaId, request.AlunoRa);
        }
    }
}
