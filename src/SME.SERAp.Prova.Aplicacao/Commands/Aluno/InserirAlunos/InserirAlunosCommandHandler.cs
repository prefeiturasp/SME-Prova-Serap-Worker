using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class InserirAlunosCommandHandler : IRequestHandler<InserirAlunosCommand, bool>
    {
        private readonly IRepositorioAlunoEntity repositorioAlunoEntity;

        public InserirAlunosCommandHandler(IRepositorioAlunoEntity repositorioAlunoEntity)
        {
            this.repositorioAlunoEntity = repositorioAlunoEntity ?? throw new System.ArgumentNullException(nameof(repositorioAlunoEntity));
        }

        public async Task<bool> Handle(InserirAlunosCommand request, CancellationToken cancellationToken)
        {
            await repositorioAlunoEntity.InserirVariosAsync(request.Alunos);
            return true;
        }

    }
}
