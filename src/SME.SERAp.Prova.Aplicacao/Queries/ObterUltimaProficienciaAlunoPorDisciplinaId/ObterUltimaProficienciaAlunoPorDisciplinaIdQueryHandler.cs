using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUltimaProficienciaAlunoPorDisciplinaIdQueryHandler : IRequestHandler<ObterUltimaProficienciaAlunoPorDisciplinaIdQuery, decimal>
    {
        private readonly IRepositorioAlunoProvaProficiencia repositorioAlunoProvaProficiencia;

        public ObterUltimaProficienciaAlunoPorDisciplinaIdQueryHandler(IRepositorioAlunoProvaProficiencia repositorioAlunoProvaProficiencia)
        {
            this.repositorioAlunoProvaProficiencia = repositorioAlunoProvaProficiencia ?? throw new ArgumentException(nameof(repositorioAlunoProvaProficiencia));
        }

        public Task<decimal> Handle(ObterUltimaProficienciaAlunoPorDisciplinaIdQuery request, CancellationToken cancellationToken)
        {
            return repositorioAlunoProvaProficiencia.ObterUltimaProficienciaAlunoPorDisciplinaIdAsync(request.AlunoId, request.DisciplinaId);
        }
    }
}
