using FluentValidation;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterQuestoesCompletasLegadoPorProvaIdParaCacheQueryValidator : AbstractValidator<ObterQuestoesCompletasLegadoPorProvaIdParaCacheQuery>
    {
        public ObterQuestoesCompletasLegadoPorProvaIdParaCacheQueryValidator()
        {
            RuleFor(c => c.ProvaId)
                .GreaterThan(0)
                .WithMessage("O Id da prova deve ser informado para obter as questões completas do legado.");
        }
    }
}