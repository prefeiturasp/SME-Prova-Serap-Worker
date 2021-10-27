using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlunoPorCodigoQueryHandler : IRequestHandler<ObterAlunoPorCodigoQuery, Aluno>
    {
        private readonly IRepositorioAluno repositorioAluno;

        public ObterAlunoPorCodigoQueryHandler(IRepositorioAluno repositorioAluno)
        {
            this.repositorioAluno = repositorioAluno ?? throw new ArgumentNullException(nameof(repositorioAluno));
        }

        public async Task<Aluno> Handle(ObterAlunoPorCodigoQuery request, CancellationToken cancellationToken)
            => await repositorioAluno.ObterAlunoPorCodigo(request.CodigoAluno);
    }
}
