using FluentValidation;
using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class RemoverQuestaoAlunoTaiPorProvaIdCommand : IRequest<bool>
    {
        public RemoverQuestaoAlunoTaiPorProvaIdCommand(long provaId)
        {
            ProvaId = provaId;
        }

        public long ProvaId { get; }
    }

    public class RemoverQuestaoAlunoTaiPorProvaIdCommandValidator : AbstractValidator<RemoverQuestaoAlunoTaiPorProvaIdCommand>
    {
        public RemoverQuestaoAlunoTaiPorProvaIdCommandValidator()
        {
            RuleFor(c => c.ProvaId)
                .GreaterThan(0)
                .WithMessage("O Id da prova deve ser informado para remover as questões TAI do aluno.");
        }
    }
}