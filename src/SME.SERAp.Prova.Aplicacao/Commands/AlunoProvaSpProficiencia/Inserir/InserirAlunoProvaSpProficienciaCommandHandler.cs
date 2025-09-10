using MediatR;
using SME.SERAp.Prova.Dados.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.Commands
{
    public class InserirAlunoProvaSpProficienciaCommandHandler : IRequestHandler<InserirAlunoProvaSpProficienciaCommand, long>
    {
        private readonly IRepositorioAlunoProvaSpProficiencia repositorioAlunoProvaSpProficiencia;
        public InserirAlunoProvaSpProficienciaCommandHandler(IRepositorioAlunoProvaSpProficiencia repositorioAlunoProvaSpProficiencia)
        {
            this.repositorioAlunoProvaSpProficiencia = repositorioAlunoProvaSpProficiencia;
        }

        public Task<long> Handle(InserirAlunoProvaSpProficienciaCommand request, CancellationToken cancellationToken)
        {
            return repositorioAlunoProvaSpProficiencia.IncluirAsync(request.AlunoProvaSpProficiencia);
        }
    }
}
