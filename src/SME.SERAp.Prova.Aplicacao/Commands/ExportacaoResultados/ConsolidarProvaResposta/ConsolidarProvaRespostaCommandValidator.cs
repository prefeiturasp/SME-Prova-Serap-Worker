using FluentValidation;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ConsolidarProvaRespostaCommandValidator : AbstractValidator<ConsolidarProvaRespostaCommand>
    {
        public ConsolidarProvaRespostaCommandValidator()
        {
            RuleFor(x => x.ProvaLegadoId).NotEmpty().WithMessage("ProvaLegadoId deve ser informado");
            RuleFor(x => x.Take).NotEmpty().WithMessage("Take deve ser informado");
            RuleFor(x => x.Skip).NotEmpty().WithMessage("Skip deve ser informado");
            RuleFor(x => x.AderirTodos).NotEmpty().WithMessage("AderirTodos deve ser informado");
            RuleFor(x => x.ParaEstudanteComDeficiencia).NotEmpty().WithMessage("ParaEstudanteComDeficiencia deve ser informado");
        }
    }
}
