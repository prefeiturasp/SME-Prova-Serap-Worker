using MediatR;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmaAlunoHistoricoSerapPorAlunosRaQuery : IRequest<IEnumerable<TurmaAlunoHistoricoDto>>
    {
        public ObterTurmaAlunoHistoricoSerapPorAlunosRaQuery(long[] alunosRa)
        {
            AlunosRa = alunosRa;
        }

        public long[] AlunosRa { get; }
    }
}
