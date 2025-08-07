using MediatR;

namespace SME.SERAp.Prova.Aplicacao.Commands
{
    public class ExcluirQuestaoAlunoTaiCommand : IRequest<int>
    {
        public long ProvaId { get; set; }

        public long AlunoRa { get; set; }

        public ExcluirQuestaoAlunoTaiCommand(long provaId, long alunoRa)
        {
            ProvaId = provaId;
            AlunoRa = alunoRa;
        }
    }
}
