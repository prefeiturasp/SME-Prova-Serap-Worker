using MediatR;
using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlunosSerapPorTurmasCodigoQuery : IRequest<IEnumerable<Aluno>>
    {
        public ObterAlunosSerapPorTurmasCodigoQuery(long[] turmasCodigo)
        {
            TurmasCodigo = turmasCodigo;
        }

        public long[] TurmasCodigo { get; set; }
    }
}
