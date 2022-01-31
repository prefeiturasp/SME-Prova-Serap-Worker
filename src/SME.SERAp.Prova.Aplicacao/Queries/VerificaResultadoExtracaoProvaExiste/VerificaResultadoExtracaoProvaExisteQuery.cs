using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class VerificaResultadoExtracaoProvaExisteQuery : IRequest<bool>
    {
        public VerificaResultadoExtracaoProvaExisteQuery(long provaLegadoId)
        {
            ProvaLegadoId = provaLegadoId;
        }
        public long ProvaLegadoId { get; set; }
    }
}
