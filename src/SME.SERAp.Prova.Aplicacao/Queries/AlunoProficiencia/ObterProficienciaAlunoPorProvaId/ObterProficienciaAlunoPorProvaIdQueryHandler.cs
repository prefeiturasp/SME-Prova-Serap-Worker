using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterProficienciaAlunoPorProvaIdQueryHandler : IRequestHandler<ObterProficienciaAlunoPorProvaIdQuery, decimal>
    {

        private readonly IRepositorioAlunoProvaProficiencia repositorioAlunoProvaProficiencia;

        public ObterProficienciaAlunoPorProvaIdQueryHandler(IRepositorioAlunoProvaProficiencia repositorioAlunoProvaProficiencia)
        {
            this.repositorioAlunoProvaProficiencia = repositorioAlunoProvaProficiencia ?? throw new ArgumentNullException(nameof(repositorioAlunoProvaProficiencia));
        }

        public async Task<decimal> Handle(ObterProficienciaAlunoPorProvaIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAlunoProvaProficiencia.ObterProficienciaInicialAlunoPorProvaIdAsync(request.ProvaId, request.AlunoId);
        }
    }
}
