using MediatR;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlunosPorTurmaCodigoQuery : IRequest<IEnumerable<AlunoEolDto>>
    {
        public ObterAlunosPorTurmaCodigoQuery(long turmaCodigo)
        {
            TurmaCodigo = turmaCodigo;
        }

        public long TurmaCodigo { get; set; }
    }
}
