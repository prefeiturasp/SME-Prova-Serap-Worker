using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;
using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlunosAdesaoPorProvaIdQueryHandler : IRequestHandler<ObterAlunosAdesaoPorProvaIdQuery, IEnumerable<Aluno>>
    {
        private readonly IRepositorioAluno repositorioAluno;

        public ObterAlunosAdesaoPorProvaIdQueryHandler(IRepositorioAluno repositorioAluno)
        {
            this.repositorioAluno = repositorioAluno ?? throw new ArgumentNullException(nameof(repositorioAluno));
        }

        public async Task<IEnumerable<Aluno>> Handle(ObterAlunosAdesaoPorProvaIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAluno.ObterAlunosAdesaoPorProvaId(request.ProvaId);
        }
    }
}
