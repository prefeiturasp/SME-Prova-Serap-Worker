using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlunosSemCadernoProvaBibQueryHandler : IRequestHandler<ObterAlunosSemCadernoProvaBibQuery, IEnumerable<ProvaCadernoAlunoDto>>
    {
        private readonly IRepositorioCadernoAluno repositorioCadernoAluno;

        public ObterAlunosSemCadernoProvaBibQueryHandler(IRepositorioCadernoAluno repositorioCadernoAluno)
        {
            this.repositorioCadernoAluno = repositorioCadernoAluno ?? throw new ArgumentNullException(nameof(repositorioCadernoAluno));
        }

        public async Task<IEnumerable<ProvaCadernoAlunoDto>> Handle(ObterAlunosSemCadernoProvaBibQuery request, CancellationToken cancellationToken)
        {
            return await repositorioCadernoAluno.ObterAlunosSemCadernosProvaBibAsync();
        }
    }
}
