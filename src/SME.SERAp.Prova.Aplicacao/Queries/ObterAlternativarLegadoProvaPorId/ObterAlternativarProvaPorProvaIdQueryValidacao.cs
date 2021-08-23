using FluentValidation;

namespace SME.SERAp.Prova.Aplicacao
{
    public class
        ObterAlternativarLegadoProvaPorProvaIdQueryValidacao : AbstractValidator<ObterAlternativarLegadoProvaPorProvaIdQuery>
    {
        public ObterAlternativarLegadoProvaPorProvaIdQueryValidacao()
        {
            RuleFor(query => query.ProvaId)
                .NotEmpty()
                .WithMessage("A Prova id não pode ser vazia");
        }
    }
}