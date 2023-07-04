using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AtualizarTurmaCommand : IRequest<long>
    {
        public AtualizarTurmaCommand(Turma turma)
        {
            Turma = turma;
        }

        public Turma Turma { get; }
    }
}