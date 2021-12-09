using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlunosSerapPorCodigosQueryHandler : IRequestHandler<ObterAlunosSerapPorCodigosQuery, IEnumerable<Aluno>>
    {
        private readonly IRepositorioAluno repositorioAluno;

        public ObterAlunosSerapPorCodigosQueryHandler(IRepositorioAluno repositorioAluno)
        {
            this.repositorioAluno = repositorioAluno ?? throw new ArgumentNullException(nameof(repositorioAluno));
        }

        public async Task<IEnumerable<Aluno>> Handle(ObterAlunosSerapPorCodigosQuery request, CancellationToken cancellationToken)
            => await repositorioAluno.ObterAlunoPorCodigosAsync(request.CodigoAlunos);
    }
}
