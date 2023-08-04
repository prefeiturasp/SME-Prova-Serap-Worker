using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterProvaDetalhesPorProvaLegadoIdQuery : IRequest<Dominio.Prova>
    {
        public ObterProvaDetalhesPorProvaLegadoIdQuery(long provaLegadoId)
        {
            ProvaLegadoId = provaLegadoId;
        }

        public long ProvaLegadoId { get; }
    }
}
