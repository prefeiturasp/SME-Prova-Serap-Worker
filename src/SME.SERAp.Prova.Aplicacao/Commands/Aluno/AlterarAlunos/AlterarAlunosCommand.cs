using MediatR;
using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AlterarAlunosCommand : IRequest<bool>
    {
        public AlterarAlunosCommand(IEnumerable<Aluno> alunos)
        {
            Alunos = alunos;
        }

        public IEnumerable<Aluno> Alunos { get; set; }
    }
}
