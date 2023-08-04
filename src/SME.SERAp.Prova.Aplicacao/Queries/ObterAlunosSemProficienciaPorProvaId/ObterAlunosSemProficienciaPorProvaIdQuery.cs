using System.Collections.Generic;
using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlunosSemProficienciaPorProvaIdQuery : IRequest<IEnumerable<AlunoProvaDto>>
    {
        public ObterAlunosSemProficienciaPorProvaIdQuery(long provaId)
        {
            ProvaId = provaId;
        }

        public long ProvaId { get; }
    }
}