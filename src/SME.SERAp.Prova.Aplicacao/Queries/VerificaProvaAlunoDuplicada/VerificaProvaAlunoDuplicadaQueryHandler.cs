using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.Queries
{
    public class VerificaProvaAlunoDuplicadaQueryHandler : IRequestHandler<VerificaProvaAlunoDuplicadaQuery, bool>
    {
        private readonly IRepositorioProvaAluno repositorioProvaAluno;
        public VerificaProvaAlunoDuplicadaQueryHandler(IRepositorioProvaAluno repositorioProvaAluno)
        {
            this.repositorioProvaAluno = repositorioProvaAluno;
        }

        public Task<bool> Handle(VerificaProvaAlunoDuplicadaQuery request, CancellationToken cancellationToken)
        {
            return this.repositorioProvaAluno.VerificaProvaAlunoDuplicada(request.ProvaId, request.AlunoRa);
        }
    }
}
