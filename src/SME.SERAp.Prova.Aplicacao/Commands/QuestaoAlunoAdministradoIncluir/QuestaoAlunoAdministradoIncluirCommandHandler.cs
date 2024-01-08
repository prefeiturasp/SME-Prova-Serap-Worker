using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;

namespace SME.SERAp.Prova.Aplicacao
{
    public class QuestaoAlunoAdministradoIncluirCommandHandler : IRequestHandler<QuestaoAlunoAdministradoIncluirCommand, long>
    {
        private readonly IRepositorioQuestaoAlunoAdministrado repositorioQuestaoAlunoAdministrado;

        public QuestaoAlunoAdministradoIncluirCommandHandler(IRepositorioQuestaoAlunoAdministrado repositorioQuestaoAlunoAdministrado)
        {
            this.repositorioQuestaoAlunoAdministrado = repositorioQuestaoAlunoAdministrado ?? throw new ArgumentNullException(nameof(repositorioQuestaoAlunoAdministrado));
        }

        public async Task<long> Handle(QuestaoAlunoAdministradoIncluirCommand request, CancellationToken cancellationToken)
        {
            return await repositorioQuestaoAlunoAdministrado.IncluirAsync(request.QuestaoAlunoAdministrado);
        }
    }
}