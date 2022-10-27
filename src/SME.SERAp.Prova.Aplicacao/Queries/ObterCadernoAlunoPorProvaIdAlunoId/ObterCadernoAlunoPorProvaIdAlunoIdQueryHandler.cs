using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterCadernoAlunoPorProvaIdAlunoIdQueryHandler : IRequestHandler<ObterCadernoAlunoPorProvaIdAlunoIdQuery, CadernoAluno>
    {
        private readonly IRepositorioCadernoAluno repositorioCadernoAluno;

        public ObterCadernoAlunoPorProvaIdAlunoIdQueryHandler(IRepositorioCadernoAluno repositorioCadernoAluno)
        {
            this.repositorioCadernoAluno = repositorioCadernoAluno ?? throw new ArgumentNullException(nameof(repositorioCadernoAluno));
        }

        public async Task<CadernoAluno> Handle(ObterCadernoAlunoPorProvaIdAlunoIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioCadernoAluno.ObterCadernoAlunoPorProvaIdAlunoIdAsync(request.ProvaId, request.AlunoId);
        }
    }
}
