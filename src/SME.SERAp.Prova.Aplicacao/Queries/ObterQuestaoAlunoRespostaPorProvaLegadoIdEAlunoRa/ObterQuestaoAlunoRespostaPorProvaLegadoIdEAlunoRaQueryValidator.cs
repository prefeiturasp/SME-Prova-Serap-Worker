using FluentValidation;

namespace SME.SERAp.Prova.Aplicacao.Queries.ObterQuestaoAlunoRespostaPorProvaIdEAlunoRa
{
    public class ObterQuestaoAlunoRespostaPorProvaLegadoIdEAlunoRaQueryValidator : AbstractValidator<ObterQuestaoAlunoRespostaPorProvaLegadoIdEAlunoRaQuery>
    {
        public ObterQuestaoAlunoRespostaPorProvaLegadoIdEAlunoRaQueryValidator()
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

