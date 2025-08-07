using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.Commands.AlunoProvaProficiencia.Excluir
{
    public class ExcluirAlunoProvaProficienciaCommandHandler : IRequestHandler<ExcluirAlunoProvaProficienciaCommand, int>
    {
        private readonly IRepositorioAlunoProvaProficiencia repositorioAlunoProvaProficiencia;
        public ExcluirAlunoProvaProficienciaCommandHandler(IRepositorioAlunoProvaProficiencia repositorioAlunoProvaProficiencia)
        {
            this.repositorioAlunoProvaProficiencia = repositorioAlunoProvaProficiencia;
        }
        public Task<int> Handle(ExcluirAlunoProvaProficienciaCommand request, CancellationToken cancellationToken)
        {
            return repositorioAlunoProvaProficiencia.ExcluirAlunoProvaProficiencia(request.ProvaId, request.AlunoRa);
        }
    }
}