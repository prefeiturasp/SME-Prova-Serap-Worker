using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlunosProvaTaiSemCadernoQueryHandler : IRequestHandler<ObterAlunosProvaTaiSemCadernoQuery, IEnumerable<ProvaAlunoTaiSemCadernoDto>>
    {

        private readonly IRepositorioAluno repositorioAluno;

        public ObterAlunosProvaTaiSemCadernoQueryHandler(IRepositorioAluno repositorioAluno)
        {
            this.repositorioAluno = repositorioAluno ?? throw new ArgumentNullException(nameof(repositorioAluno));
        }
        public async Task<IEnumerable<ProvaAlunoTaiSemCadernoDto>> Handle(ObterAlunosProvaTaiSemCadernoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAluno.ObterAlunosProvaTaiSemCaderno();
        }
    }
}

