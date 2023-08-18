using MediatR;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
  public  class ObterAlunosProvaTaiSemCadernoQuery : IRequest<IEnumerable<ProvaAlunoTaiSemCadernoDto>>
    {
        public ObterAlunosProvaTaiSemCadernoQuery(long provaId, string ano)
        {
            ProvaId = provaId;
            Ano = ano;
        }

        public long ProvaId { get; }
        public string Ano { get; }
    }
}

