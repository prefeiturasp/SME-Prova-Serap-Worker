using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlunosSerapPorTurmasCodigoQueryHandler : IRequestHandler<ObterAlunosSerapPorTurmasCodigoQuery, IEnumerable<Aluno>>
    {
        private readonly IRepositorioAluno repositorioAluno;

        public ObterAlunosSerapPorTurmasCodigoQueryHandler(IRepositorioAluno repositorioAluno)
        {
            this.repositorioAluno = repositorioAluno ?? throw new ArgumentNullException(nameof(repositorioAluno));
        }
        public async Task<IEnumerable<Aluno>> Handle(ObterAlunosSerapPorTurmasCodigoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAluno.ObterAlunosPorTurmasCodigoAsync(request.TurmasCodigo);
        }
    }
}
