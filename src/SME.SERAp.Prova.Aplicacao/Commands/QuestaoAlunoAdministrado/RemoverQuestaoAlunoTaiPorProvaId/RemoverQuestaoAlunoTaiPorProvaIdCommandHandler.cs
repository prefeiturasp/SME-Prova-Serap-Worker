using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;

namespace SME.SERAp.Prova.Aplicacao
{
    public class RemoverQuestaoAlunoTaiPorProvaIdCommandHandler : IRequestHandler<RemoverQuestaoAlunoTaiPorProvaIdCommand, bool>
    {
        private readonly IRepositorioQuestaoAlunoTai repositorioQuestaoAlunoTai;

        public RemoverQuestaoAlunoTaiPorProvaIdCommandHandler(IRepositorioQuestaoAlunoTai repositorioQuestaoAlunoTai)
        {
            this.repositorioQuestaoAlunoTai = repositorioQuestaoAlunoTai ?? throw new ArgumentNullException(nameof(repositorioQuestaoAlunoTai));
        }

        public async Task<bool> Handle(RemoverQuestaoAlunoTaiPorProvaIdCommand request, CancellationToken cancellationToken)
        {
            return await repositorioQuestaoAlunoTai.RemoverQuestaoAlunoTaiPorProvaIdAsync(request.ProvaId);
        }
    }
}