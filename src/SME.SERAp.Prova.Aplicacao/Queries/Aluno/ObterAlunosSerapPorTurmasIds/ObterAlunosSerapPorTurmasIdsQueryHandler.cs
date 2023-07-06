using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlunosSerapPorTurmasIdsQueryHandler : IRequestHandler<ObterAlunosSerapPorTurmasIdsQuery, IEnumerable<Aluno>>
    {
        private readonly IRepositorioAluno repositorioAluno;

        public ObterAlunosSerapPorTurmasIdsQueryHandler(IRepositorioAluno repositorioAluno)
        {
            this.repositorioAluno = repositorioAluno ?? throw new ArgumentNullException(nameof(repositorioAluno));
        }

        public async Task<IEnumerable<Aluno>> Handle(ObterAlunosSerapPorTurmasIdsQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAluno.ObterAlunosPorTurmasIdsAsync(request.TurmasIds);
        }
    }
}
