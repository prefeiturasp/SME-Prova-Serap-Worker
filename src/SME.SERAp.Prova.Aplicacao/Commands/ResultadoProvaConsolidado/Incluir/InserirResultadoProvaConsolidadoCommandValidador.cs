using FluentValidation;

namespace SME.SERAp.Prova.Aplicacao
{
    public class InserirResultadoProvaConsolidadoCommandValidador : AbstractValidator<InserirResultadoProvaConsolidadoCommand>
    {
        public InserirResultadoProvaConsolidadoCommandValidador()
        {
            RuleFor(x => x.ResultadoProvaConsolidado)
                .NotNull().NotEmpty()
                .WithMessage("O Objeto ResultadoProvaConsolidado é obrigatório");
        }
    }
}