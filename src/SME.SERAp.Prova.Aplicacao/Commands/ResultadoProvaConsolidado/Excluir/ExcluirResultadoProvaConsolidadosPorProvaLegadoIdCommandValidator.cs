using FluentValidation;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExcluirResultadoProvaConsolidadosPorProvaLegadoIdCommandValidator : AbstractValidator<ExcluirResultadoProvaConsolidadosPorProvaLegadoIdCommand>
    {
        public ExcluirResultadoProvaConsolidadosPorProvaLegadoIdCommandValidator()
        {
            RuleFor(x => x.ProvaLegadoId)
                .NotEmpty()
                .WithMessage("Código da prova é obrigatório");
        }
    }
}

