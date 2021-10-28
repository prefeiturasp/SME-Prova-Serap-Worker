using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ProvaRemoverCadernoAlunosPorProvaIdHandler : IRequestHandler<ProvaRemoverCadernoAlunosPorProvaId, bool>
    {
        private readonly IRepositorioCadernoAluno repositorioCadernoAluno;

        public ProvaRemoverCadernoAlunosPorProvaIdHandler(IRepositorioCadernoAluno repositorioCadernoAluno)
        {
            this.repositorioCadernoAluno = repositorioCadernoAluno ?? throw new ArgumentNullException(nameof(repositorioCadernoAluno));
        }
        public async Task<bool> Handle(ProvaRemoverCadernoAlunosPorProvaId request, CancellationToken cancellationToken)
        {
            return await repositorioCadernoAluno.RemoverCadernosPorProvaIdAsync(request.Id);
        }
    }
}
