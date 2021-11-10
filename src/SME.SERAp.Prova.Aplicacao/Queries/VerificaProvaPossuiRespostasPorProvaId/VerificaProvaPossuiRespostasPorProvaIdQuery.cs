using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class VerificaProvaPossuiRespostasPorProvaIdQuery : IRequest<bool>
    {
        public VerificaProvaPossuiRespostasPorProvaIdQuery(long provaId)
        {
            ProvaId = provaId;
        }

        public long ProvaId { get; set; }
    }
}
