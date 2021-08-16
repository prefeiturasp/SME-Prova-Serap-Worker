using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterProvaLegadoDetalhesPorIdQuery : IRequest<ProvaLegadoDetalhesIdDto>
    {
        public ObterProvaLegadoDetalhesPorIdQuery(long provaId)
        {
            ProvaId = provaId;
        }

        public long ProvaId { get; set; }
    }
}
