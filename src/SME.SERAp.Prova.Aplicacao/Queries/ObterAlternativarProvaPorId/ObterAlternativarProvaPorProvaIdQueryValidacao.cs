using FluentValidation;

namespace SME.SERAp.Prova.Aplicacao
{
    public class
        ObterAlternativarProvaPorProvaIdQueryValidacao : AbstractValidator<ObterAlternativarProvaPorProvaIdQuery>
    {
        public ObterAlternativarProvaPorProvaIdQueryValidacao()
        {
            RuleFor(query => query.ProvaId)
                .NotEmpty()
                .WithMessage("A Prova id não pode ser vazia");
        }
    }
}