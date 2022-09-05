using MediatR;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlunosSemCadernoProvaBibQuery : IRequest<IEnumerable<ProvaCadernoAlunoDto>>
    {
        public ObterAlunosSemCadernoProvaBibQuery(long provaId)
        {
            ProvaId = provaId;
        }

        public long ProvaId { get; set; }
    }
}
