using MediatR;
using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AlterarTurmasCommand : IRequest<bool>
    {
        public AlterarTurmasCommand(IEnumerable<Turma> turmas)
        {
            Turmas = turmas;
        }

        public IEnumerable<Turma> Turmas { get; set; }
    }
}
