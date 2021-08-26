using FluentValidation;

namespace SME.SERAp.Prova.Aplicacao
{
    public class
        ObterAlternativarLegadoProvaPorProvaIdQueryValidacao : AbstractValidator<ObterAlternativarLegadoProvaPorProvaIdQuery>
    {
        public ObterAlternativarLegadoProvaPorProvaIdQueryValidacao()
        {
            RuleFor(query => query.QuestaoId)
                .NotEmpty()
                .WithMessage("A Questão id não pode ser vazia");
        }
    }
}