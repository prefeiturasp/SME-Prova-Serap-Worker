using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AlterarAlunosCommandHandler : IRequestHandler<AlterarAlunosCommand, bool>
    {
        private readonly IRepositorioAlunoEntity repositorioAlunoEntity;

        public AlterarAlunosCommandHandler(IRepositorioAlunoEntity repositorioAlunoEntity)
        {
            this.repositorioAlunoEntity = repositorioAlunoEntity ?? throw new System.ArgumentNullException(nameof(repositorioAlunoEntity));
        }

        public async Task<bool> Handle(AlterarAlunosCommand request, CancellationToken cancellationToken)
        {
            await repositorioAlunoEntity.AlterarVariosAsync(request.Alunos);
            return true;
        }

    }
}
