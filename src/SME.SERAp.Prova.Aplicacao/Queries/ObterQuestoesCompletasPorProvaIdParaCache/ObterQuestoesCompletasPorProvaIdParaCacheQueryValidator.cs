using FluentValidation;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterQuestoesCompletasPorProvaIdParaCacheQueryValidator : AbstractValidator<ObterQuestoesCompletasPorProvaIdParaCacheQuery>
    {
        public ObterQuestoesCompletasPorProvaIdParaCacheQueryValidator()
        {
            RuleFor(c => c.ProvaId)
                .GreaterThan(0)
                .WithMessage("O Id da prova deve ser informado para obter as questões completas.");
        }
    }
}