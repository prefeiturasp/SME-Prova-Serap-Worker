using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Aplicacao.Queries.Questao.ExisteQuestaoAlunoTaiPorId
{
    public class ExisteQuestaoAlunoTaiPorAlunoIdQuery : IRequest<bool>
    {
        public ExisteQuestaoAlunoTaiPorAlunoIdQuery(long alunoId)
        {
            AlunoId = alunoId;
        }
        public long AlunoId { get; }
    }

    public class ExisteQuestaoAlunoTaiPorAlunoIdQueryValidator : AbstractValidator<ExisteQuestaoAlunoTaiPorAlunoIdQuery>
    {
        public ExisteQuestaoAlunoTaiPorAlunoIdQueryValidator()
        {
            RuleFor(x => x.AlunoId)
                .GreaterThan(0)
                .WithMessage(" Informe o id do aluno para verificar se existe o caderno do aluno");
        }
    }
}
