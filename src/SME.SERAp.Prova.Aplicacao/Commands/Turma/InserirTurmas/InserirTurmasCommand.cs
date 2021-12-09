using MediatR;
using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class InserirTurmasCommand : IRequest<bool>
    {
        public InserirTurmasCommand(IEnumerable<Turma> turmas)
        {
            Turmas = turmas;
        }

        public IEnumerable<Turma> Turmas { get; set; }
    }
}
