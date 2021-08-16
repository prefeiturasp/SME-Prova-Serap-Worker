using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterProvaDetalhesPorIdQuery : IRequest<Dominio.Prova>
    {
        public ObterProvaDetalhesPorIdQuery(long provaId)
        {
            ProvaId = provaId;
        }

        public long ProvaId { get; set; }
    }
}
