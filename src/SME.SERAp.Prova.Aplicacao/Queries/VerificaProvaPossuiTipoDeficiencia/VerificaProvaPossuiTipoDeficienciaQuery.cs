using MediatR;

namespace SME.SERAp.Prova.Aplicacao.Queries.VerificaProvaPossuiTipoDeficiencia
{
    public class VerificaProvaPossuiTipoDeficienciaQuery : IRequest<bool>
    {
        public VerificaProvaPossuiTipoDeficienciaQuery(long provaLegadoId)
        {
            ProvaLegadoId = provaLegadoId;
        }

        public long ProvaLegadoId { get; }
    }
}