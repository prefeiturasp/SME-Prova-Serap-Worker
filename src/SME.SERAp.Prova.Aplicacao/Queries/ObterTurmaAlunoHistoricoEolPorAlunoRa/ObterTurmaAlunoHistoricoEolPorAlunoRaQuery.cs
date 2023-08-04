using MediatR;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmaAlunoHistoricoEolPorAlunosRaQuery : IRequest<IEnumerable<TurmaEolDto>>
    {
        public ObterTurmaAlunoHistoricoEolPorAlunosRaQuery(long[] alunoRa)
        {
            AlunoRa = alunoRa;
        }

        public long[] AlunoRa { get; }
    }
}
