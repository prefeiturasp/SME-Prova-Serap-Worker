using FluentValidation;
using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class QuestaoAlunoAdministradoIncluirCommand : IRequest<long>
    {
        public QuestaoAlunoAdministradoIncluirCommand(QuestaoAlunoAdministrado questaoAlunoAdministrado)
        {
            QuestaoAlunoAdministrado = questaoAlunoAdministrado;
        }

        public QuestaoAlunoAdministrado QuestaoAlunoAdministrado { get; }
    }

    public class QuestaoAlunoAdministradoIncluirCommandValidator : AbstractValidator<QuestaoAlunoAdministradoIncluirCommand>
    {
        public QuestaoAlunoAdministradoIncluirCommandValidator()
        {
            RuleFor(c => c.QuestaoAlunoAdministrado)
                .NotNull()
                .WithMessage("Os dados do administrado da questão do aluno devem ser informados.")
                .DependentRules(() =>
                {
                    RuleFor(c => c.QuestaoAlunoAdministrado.QuestaoId)
                        .GreaterThan(0)
                        .WithMessage("O Id da questão deve ser informado.");

                    RuleFor(c => c.QuestaoAlunoAdministrado.AlunoId)
                        .GreaterThan(0)
                        .WithMessage("O Id do aluno deve ser informado.");

                    RuleFor(c => c.QuestaoAlunoAdministrado.Ordem)
                        .GreaterThanOrEqualTo(0)
                        .WithMessage("A ordem deve ser maior ou igual a 0 (zero).");

                    RuleFor(c => c.QuestaoAlunoAdministrado.CriadoEm)
                        .NotNull()
                        .WithMessage("A data de criação do registro deve ser informada.");
                });
        }
    }
}