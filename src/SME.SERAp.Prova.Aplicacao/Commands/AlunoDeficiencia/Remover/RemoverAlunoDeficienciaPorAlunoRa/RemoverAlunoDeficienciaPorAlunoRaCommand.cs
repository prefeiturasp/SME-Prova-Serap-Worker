using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class RemoverAlunoDeficienciaPorAlunoRaCommand : IRequest<bool>
    {
        public RemoverAlunoDeficienciaPorAlunoRaCommand(long alunoRa)
        {
            AlunoRa = alunoRa;
        }

        public long AlunoRa { get; set; }
    }
}
