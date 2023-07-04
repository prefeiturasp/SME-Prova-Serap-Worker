using FluentValidation;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AtualizarTurmaCommandValidator : AbstractValidator<AtualizarTurmaCommand>
    {
        public AtualizarTurmaCommandValidator()
        {
            RuleFor(c => c.Turma)
                .NotNull()
                .WithMessage("Os dados da turma devem ser informados.");
        }
    }
}