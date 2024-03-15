using FluentValidation;
using SME.SERAp.Prova.Aplicacao.Queries.ObterAlunosResultadoProvaAdesao;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Aplicacao.Queries.ObterQuestaoAlunoRespostaPorProvaIdEAlunoRa
{
    public class ObterQuestaoAlunoRespostaPorProvaIdEAlunoRaQueryValidator : AbstractValidator<ObterQuestaoAlunoRespostaPorProvaIdEAlunoRaQuery>
    {
        public ObterQuestaoAlunoRespostaPorProvaIdEAlunoRaQueryValidator()
        {
            RuleFor(x => x.AlunoRa)
                .NotEmpty()
                    .WithMessage("Código do aluno é obrigatório");

            RuleFor(x => x.ProvaLegadoId)
         .NotEmpty()
             .WithMessage("Código da prova é obrigatório");
        }
    }
}

