using MediatR;
using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlunosSerapPorTurmasIdsQuery : IRequest<IEnumerable<Aluno>>
    {
        public ObterAlunosSerapPorTurmasIdsQuery(long[] turmasIds)
        {
            TurmasIds = turmasIds;
        }

        public long[] TurmasIds { get; }
    }
}
