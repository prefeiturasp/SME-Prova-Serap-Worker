using MediatR;
using SME.SERAp.Prova.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Aplicacao
{
    public class InativarAlunosCommand : IRequest<bool>
    {
        public InativarAlunosCommand(long turmaId, IEnumerable<Aluno> alunos)
        {
            TurmaId = turmaId;
            Alunos = alunos;
        }

        public long TurmaId { get; set; }
        public IEnumerable<Aluno> Alunos { get; set; }

    }
}
