using MediatR;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterExtracaoProvaRespostaQuery : IRequest<IEnumerable<ConsolidadoProvaRespostaDto>>
    {
        public long ProvaSerapId { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }

        public ObterExtracaoProvaRespostaQuery(long provaSerapId, int take, int skip)
        {
            ProvaSerapId = provaSerapId;
            Take = take;
            Skip = skip;
        }
    }
}