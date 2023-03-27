using FluentValidation;

namespace SME.SERAp.Prova.Aplicacao
{
    public class FinalizarProcessosPorTipoCommandValidator : AbstractValidator<FinalizarProcessosPorTipoCommand>
    {
        public FinalizarProcessosPorTipoCommandValidator()
        {
            RuleFor(x => x.TipoResultadoProcesso).IsInEnum()
                .WithMessage("TipoResultadoProcesso inválido");
        }
    }
}
