using MediatR;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SME.SERAp.Prova.Dados.Interfaces;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmaAlunoHistoricoEolPorAlunosRaQueryHandler : IRequestHandler<ObterTurmaAlunoHistoricoEolPorAlunosRaQuery, IEnumerable<AlunoMatriculaTurmaDreDto>>
    {
        private readonly IRepositorioElasticTurma repositorioElasticTurma;

        public ObterTurmaAlunoHistoricoEolPorAlunosRaQueryHandler(IRepositorioElasticTurma repositorioElasticTurma)
        {
            this.repositorioElasticTurma = repositorioElasticTurma ?? throw new ArgumentNullException(nameof(repositorioElasticTurma));
        }

        public async Task<IEnumerable<AlunoMatriculaTurmaDreDto>> Handle(ObterTurmaAlunoHistoricoEolPorAlunosRaQuery request, CancellationToken cancellationToken)
            => await repositorioElasticTurma.ObterTurmasAlunoHistoricoPorAlunosRa(request.AlunoRa);
    }
}
