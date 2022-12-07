using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AtualizarValorProficienciaAlunoCommandHandler : IRequestHandler<AtualizarValorProficienciaAlunoCommand, bool>
    {
        private readonly IRepositorioAlunoProvaProficiencia repositorioAlunoProvaProficiencia;

        public AtualizarValorProficienciaAlunoCommandHandler(IRepositorioAlunoProvaProficiencia repositorioAlunoProvaProficiencia)
        {
            this.repositorioAlunoProvaProficiencia = repositorioAlunoProvaProficiencia ?? throw new ArgumentException(nameof(repositorioAlunoProvaProficiencia));
        }

        public async Task<bool> Handle(AtualizarValorProficienciaAlunoCommand request, CancellationToken cancellationToken)
        {
            return await repositorioAlunoProvaProficiencia.AtualizarValorProficienciaAluno(request.AlunoProvaProficiencia);
        }
    }
}
