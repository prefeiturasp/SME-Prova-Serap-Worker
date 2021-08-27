using FluentValidation;

namespace SME.SERAp.Prova.Aplicacao
{
    public class
        ObterQuestaoPorProvaLegadoQueryValidacao : AbstractValidator<ObterQuestaoPorProvaLegadoQuery>
    {
        public ObterQuestaoPorProvaLegadoQueryValidacao()
        {
            RuleFor(query => query.QuestaoId)
                .NotEmpty()
                .WithMessage("A Prova id não pode ser vazia");
        }
    }
}