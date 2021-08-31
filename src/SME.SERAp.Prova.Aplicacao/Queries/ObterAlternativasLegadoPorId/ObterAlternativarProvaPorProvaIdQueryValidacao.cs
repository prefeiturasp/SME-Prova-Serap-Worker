using FluentValidation;

namespace SME.SERAp.Prova.Aplicacao
{
    public class
        ObterAlternativasLegadoPorIdQueryValidacao : AbstractValidator<ObterAlternativasLegadoPorIdQuery>
    {
        public ObterAlternativasLegadoPorIdQueryValidacao()
        {
            RuleFor(query => query.QuestaoId)
                .NotEmpty()
                .WithMessage("A Questão id não pode ser vazia");
        }
    }
}