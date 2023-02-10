using FluentValidation;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ProvaRemoverQuestoesTriPorIdCommandValidator : AbstractValidator<ProvaRemoverQuestoesTriPorIdCommand>
    {
        public ProvaRemoverQuestoesTriPorIdCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Código da prova é obrigatório");
        }
    }
}
