using FluentValidation;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterProvaDetalhesPorProvaLegadoIdQueryValidator : AbstractValidator<ObterProvaDetalhesPorProvaLegadoIdQuery>
    {
        public ObterProvaDetalhesPorProvaLegadoIdQueryValidator()
        {
            RuleFor(c => c.ProvaLegadoId)
                .GreaterThan(0)
                .WithMessage("O Id da prova legado deve ser informado.");
        }
    }
}