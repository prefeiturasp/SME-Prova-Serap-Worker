using MediatR;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
  public  class ObterAlunosProvaTaiSemCadernoQuery : IRequest<IEnumerable<ProvaAlunoTaiSemCadernoDto>>
    {
        public ObterAlunosProvaTaiSemCadernoQuery(long provaId)
        {
            ProvaId = provaId;
        }

        public long ProvaId { get; }
    }
}

