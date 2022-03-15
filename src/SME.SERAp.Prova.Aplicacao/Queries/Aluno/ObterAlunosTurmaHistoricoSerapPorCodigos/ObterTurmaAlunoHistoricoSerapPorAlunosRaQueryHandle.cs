using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmaAlunoHistoricoSerapPorAlunosRaQueryHandle : IRequestHandler<ObterTurmaAlunoHistoricoSerapPorAlunosRaQuery, IEnumerable<TurmaAlunoHistoricoDto>>
    {
        private readonly IRepositorioTurmaAlunoHistorico repositorioTurmaAlunoHistorico;

        public ObterTurmaAlunoHistoricoSerapPorAlunosRaQueryHandle(IRepositorioTurmaAlunoHistorico repositorioTurmaAlunoHistorico)
        {
            this.repositorioTurmaAlunoHistorico = repositorioTurmaAlunoHistorico ?? throw new ArgumentNullException(nameof(repositorioTurmaAlunoHistorico));
        }

        public async Task<IEnumerable<TurmaAlunoHistoricoDto>> Handle(ObterTurmaAlunoHistoricoSerapPorAlunosRaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTurmaAlunoHistorico.ObterTurmaAlunoHistoricoPorAlunosRa(request.AlunosRa);
        }
    }
}
