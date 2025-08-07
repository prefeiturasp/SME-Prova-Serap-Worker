using MediatR;

namespace SME.SERAp.Prova.Aplicacao.Commands
{
    public class ExcluirRespostaAlunoCommand : IRequest<int>
    {
        public long ProvaId { get; set; }

        public long AlunoRa { get; set; }

        public ExcluirRespostaAlunoCommand(long provaId, long alunoRa)
        {
            ProvaId = provaId;
            AlunoRa = alunoRa;
        }
    }
}
