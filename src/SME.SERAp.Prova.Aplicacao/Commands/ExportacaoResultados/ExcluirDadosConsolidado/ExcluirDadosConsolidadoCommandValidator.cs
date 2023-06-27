using FluentValidation;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExcluirDadosConsolidadoCommandValidator : AbstractValidator<ExcluirDadosConsolidadoCommand>
    {
        public ExcluirDadosConsolidadoCommandValidator()
        {
            RuleFor(x => x.ProvaLegadoId).NotEmpty().WithMessage("ProvaLegadoId deve ser informado");
            RuleFor(x => x.Take).NotEmpty().WithMessage("Take deve ser informado");
            RuleFor(x => x.Skip).NotEmpty().WithMessage("Skip deve ser informado");
        }
    }
}
