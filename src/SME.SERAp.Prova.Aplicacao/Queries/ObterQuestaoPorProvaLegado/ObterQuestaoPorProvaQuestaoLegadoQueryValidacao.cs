using FluentValidation;

namespace SME.SERAp.Prova.Aplicacao
{
    public class
        ObterQuestaoPorProvaQuestaoLegadoQueryValidacao : AbstractValidator<ObterQuestaoPorProvaQuestaoLegadoQuery>
    {
        public ObterQuestaoPorProvaQuestaoLegadoQueryValidacao()
        {
            RuleFor(query => query.QuestaoId)
                .NotEmpty()
                .WithMessage("A Prova id não pode ser vazia");
        }
    }
}