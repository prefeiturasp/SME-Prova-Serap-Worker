using MediatR;
using SME.SERAp.Prova.Dados.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.Commands
{
    public class ExcluirAlunoProvaSpProficienciaCommandHandler : IRequestHandler<ExcluirAlunoProvaSpProficienciaCommand, int>
    {
        private readonly IRepositorioAlunoProvaSpProficiencia repositorio;
        public ExcluirAlunoProvaSpProficienciaCommandHandler(IRepositorioAlunoProvaSpProficiencia repositorio)
        {
            this.repositorio = repositorio;
        }

        public Task<int> Handle(ExcluirAlunoProvaSpProficienciaCommand request, CancellationToken cancellationToken)
        {
            return repositorio.ExcluirAlunoProvaSpProficiencia(request.AnoLetivo, request.DisciplinaId, request.AlunoRa);
        }
    }
}
