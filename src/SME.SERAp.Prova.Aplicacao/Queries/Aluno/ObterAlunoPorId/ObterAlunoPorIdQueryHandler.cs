using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlunoPorIdQueryHandler : IRequestHandler<ObterAlunoPorIdQuery, Aluno>
    {

        private readonly IRepositorioAluno repositorioAluno;

        public ObterAlunoPorIdQueryHandler(IRepositorioAluno repositorioAluno)
        {
            this.repositorioAluno = repositorioAluno ?? throw new ArgumentNullException(nameof(repositorioAluno));
        }

        public async Task<Aluno> Handle(ObterAlunoPorIdQuery request, CancellationToken cancellationToken)
            => await repositorioAluno.ObterAlunoPorIdAsync(request.AlunoId);
    }
}
