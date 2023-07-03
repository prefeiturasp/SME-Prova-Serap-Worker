using FluentValidation;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlunosSemProficienciaPorProvaIdQueryValidator : AbstractValidator<ObterAlunosSemProficienciaPorProvaIdQuery>
    {
        public ObterAlunosSemProficienciaPorProvaIdQueryValidator()
        {
            RuleFor(c => c.ProvaId)
                .GreaterThan(0)
                .WithMessage("O Id da prova deve ser informado.");
        }
    }
}