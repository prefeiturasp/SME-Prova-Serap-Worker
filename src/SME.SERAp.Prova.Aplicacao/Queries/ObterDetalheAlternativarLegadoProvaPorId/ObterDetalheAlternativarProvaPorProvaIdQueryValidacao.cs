using FluentValidation;

namespace SME.SERAp.Prova.Aplicacao
{
    public class
        ObterDetalheAlternativarLegadoProvaPorProvaIdQueryValidacao : AbstractValidator<ObterDetalheAlternativarLegadoProvaPorProvaIdQuery>
    {
        public ObterDetalheAlternativarLegadoProvaPorProvaIdQueryValidacao()
        {
            RuleFor(query => query.ProvaId)
                .NotEmpty()
                .WithMessage("A Prova id não pode ser vazia");
            
            RuleFor(query => query.QuestaoId)
                .NotEmpty()
                .WithMessage("A Questao id não pode ser vazia");
            
            RuleFor(query => query.AlternativaId)
                .NotEmpty()
                .WithMessage("A Alternativa id não pode ser vazia");
        }
    }
}