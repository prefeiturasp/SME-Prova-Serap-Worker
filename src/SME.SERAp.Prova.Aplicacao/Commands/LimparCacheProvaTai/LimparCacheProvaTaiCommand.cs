using MediatR;

namespace SME.SERAp.Prova.Aplicacao.Commands
{
    public class LimparCacheProvaTaiCommand : IRequest<bool>
    {
        public long ProvaId { get; set; }

        public long AlunoRa { get; set; }

        public LimparCacheProvaTaiCommand(long provaId, long alunoRa)
        {
            ProvaId = provaId;
            AlunoRa = alunoRa;
        }
    }
}
