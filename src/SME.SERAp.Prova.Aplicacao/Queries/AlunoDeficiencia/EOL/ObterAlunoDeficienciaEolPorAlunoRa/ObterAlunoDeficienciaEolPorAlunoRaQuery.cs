using MediatR;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlunoDeficienciaEolPorAlunoRaQuery : IRequest<IEnumerable<int>>
    {

        public long AlunoRa { get; private set; }

        public ObterAlunoDeficienciaEolPorAlunoRaQuery(long alunoRa)
        {
            AlunoRa = alunoRa;
        }
    }
}
