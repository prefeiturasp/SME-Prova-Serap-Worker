using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class IncluirAlunoProvaProficienciaCommandHandler : IRequestHandler<IncluirAlunoProvaProficienciaCommand, long>
    {
        private readonly IRepositorioAlunoProvaProficiencia repositorioAlunoProvaProficiencia;

        public IncluirAlunoProvaProficienciaCommandHandler(IRepositorioAlunoProvaProficiencia repositorioAlunoProvaProficiencia)
        {
            this.repositorioAlunoProvaProficiencia = repositorioAlunoProvaProficiencia ?? throw new ArgumentException(nameof(repositorioAlunoProvaProficiencia));
        }

        public async Task<long> Handle(IncluirAlunoProvaProficienciaCommand request, CancellationToken cancellationToken)
        {
            return await repositorioAlunoProvaProficiencia.IncluirAsync(request.AlunoProvaProficiencia);
        }
    }
}
