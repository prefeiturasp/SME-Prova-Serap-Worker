using MediatR;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlunosSemProficienciaQuery : IRequest<IEnumerable<AlunoProvaDto>>
    {
        public ObterAlunosSemProficienciaQuery(long provaId)
        {
            ProvaId = provaId;
        }

        public long ProvaId { get; set; }
    }
}
