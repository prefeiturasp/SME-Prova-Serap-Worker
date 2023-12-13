using FluentValidation;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterQuestaoCompletaPorQuestaoIdQueryValidator : AbstractValidator<ObterQuestaoCompletaPorQuestaoIdQuery>
    {
        public ObterQuestaoCompletaPorQuestaoIdQueryValidator()
        {
            RuleFor(c => c.QuestaoId)
                .GreaterThan(0)
                .WithMessage("O Id da questão deve ser informado para obter a questão completa.");
        }
    }
}