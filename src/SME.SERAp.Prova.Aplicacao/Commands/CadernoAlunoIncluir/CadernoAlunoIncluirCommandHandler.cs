using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class CadernoAlunoIncluirCommandHandler : IRequestHandler<CadernoAlunoIncluirCommand, long>
    {
        private readonly IRepositorioCadernoAluno repositorioCadernoAluno;

        public CadernoAlunoIncluirCommandHandler(IRepositorioCadernoAluno repositorioCadernoAluno)
        {
            this.repositorioCadernoAluno = repositorioCadernoAluno ?? throw new System.ArgumentNullException(nameof(repositorioCadernoAluno));
        }
        public async Task<long> Handle(CadernoAlunoIncluirCommand request, CancellationToken cancellationToken)
        {
            return await repositorioCadernoAluno.IncluirAsync(request.CadernoAluno);
        }
    }
}
