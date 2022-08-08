using MediatR;
using SME.SERAp.Prova.Dados;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class InativarAlunosCommandHandler : IRequestHandler<InativarAlunosCommand, bool>
    {
        private readonly IRepositorioAluno repositorioAluno;

        public InativarAlunosCommandHandler(IRepositorioAluno repositorioAluno)
        {
            this.repositorioAluno = repositorioAluno ?? throw new System.ArgumentNullException(nameof(repositorioAluno));
        }

        public async Task<bool> Handle(InativarAlunosCommand request, CancellationToken cancellationToken)
        {
            await repositorioAluno.InativarAlunoPorIdETurmaIdAsync(request.TurmaId, request.Alunos.Select(t => t.Id).ToArray());
            return true;
        }
    }
}
