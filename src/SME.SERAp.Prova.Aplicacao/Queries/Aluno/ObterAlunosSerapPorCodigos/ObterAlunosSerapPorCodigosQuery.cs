using MediatR;
using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlunosSerapPorCodigosQuery : IRequest<IEnumerable<Aluno>>
    {
        public ObterAlunosSerapPorCodigosQuery(long[] codigoAlunos)
        {
            CodigoAlunos = codigoAlunos;
        }

        public long[] CodigoAlunos { get; set; }
    }
}
