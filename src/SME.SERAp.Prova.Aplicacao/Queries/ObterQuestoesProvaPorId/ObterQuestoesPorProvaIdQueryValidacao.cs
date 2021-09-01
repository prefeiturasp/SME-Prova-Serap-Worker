using FluentValidation;

namespace SME.SERAp.Prova.Aplicacao
{
    public class
        ObterQuestoesPorProvaIdQueryValidacao : AbstractValidator<ObterQuestoesPorProvaIdQuery>
    {
        public ObterQuestoesPorProvaIdQueryValidacao()
        {
            RuleFor(query => query.ProvaId)
                .NotEmpty()
                .WithMessage("A Prova id não pode ser vazia");
        }
    }
}