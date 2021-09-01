using FluentValidation;

namespace SME.SERAp.Prova.Aplicacao
{
    public class
        ObterAlternativaDetalheLegadoPorIdQueryValidacao : AbstractValidator<ObterAlternativaDetalheLegadoPorIdQuery>
    {
        public ObterAlternativaDetalheLegadoPorIdQueryValidacao()
        {
            
            RuleFor(query => query.QuestaoId)
                .NotEmpty()
                .WithMessage("A Questao id não pode ser vazia");
            
            RuleFor(query => query.AlternativaId)
                .NotEmpty()
                .WithMessage("A Alternativa id não pode ser vazia");
        }
    }
}