using FluentValidation;

namespace SME.SERAp.Prova.Aplicacao
{
    public class
        ObterExtracaoProvaRespostaQueryValidacao : AbstractValidator<ObterExtracaoProvaRespostaQuery>
    {
        public ObterExtracaoProvaRespostaQueryValidacao()
        {
            
            RuleFor(query => query.ProvaSerapId)
                .NotEmpty()
                .WithMessage("O id da prova serap deve ser informado");
        }
    }
}