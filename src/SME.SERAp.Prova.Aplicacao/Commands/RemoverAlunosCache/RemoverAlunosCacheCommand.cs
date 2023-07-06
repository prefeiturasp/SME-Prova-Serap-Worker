using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class RemoverAlunosCacheCommand : IRequest<bool>
    {
        public RemoverAlunosCacheCommand(long[] alunosRA)
        {
            AlunosRA = alunosRA;
        }

        public long[] AlunosRA { get; }
    }
}
