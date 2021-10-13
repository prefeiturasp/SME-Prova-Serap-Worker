using FluentValidation;

namespace SME.SERAp.Prova.Aplicacao
{
    public class
        ObterQuestaoDetalheLegadoPorIdQueryValidacao : AbstractValidator<ObterQuestaoDetalheLegadoPorIdQuery>
    {
        public ObterQuestaoDetalheLegadoPorIdQueryValidacao()
        {
            RuleFor(query => query.ProvaLegadoId)
                .NotEmpty()
                .WithMessage("A Prova id não pode ser vazia");
            
            RuleFor(query => query.QuestaoLegadoId)
                .NotEmpty()
                .WithMessage("A Questao id não pode ser vazia");
        }
    }
}