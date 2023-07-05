using MediatR;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlunosEolPorTurmasCodigoQuery : IRequest<IEnumerable<AlunoEolDto>>
    {
        public ObterAlunosEolPorTurmasCodigoQuery(long[] turmasCodigo)
        {
            TurmasCodigo = turmasCodigo;
        }

        public long[] TurmasCodigo { get; }
    }
}
