using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class CriarProvaRespostasExtracaoCommand : IRequest<bool>
    {
        public CriarProvaRespostasExtracaoCommand(long provaId)
        {
            ProvaId = provaId;
        }

        public long ProvaId { get; set; }
    }
}
