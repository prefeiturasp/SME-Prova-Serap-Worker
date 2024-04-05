using FluentValidation;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterResumoQuestoesPorProvaIdParaCacheQueryValidator : AbstractValidator<ObterResumoQuestoesPorProvaIdParaCacheQuery>
    {
        public ObterResumoQuestoesPorProvaIdParaCacheQueryValidator()
        {
            RuleFor(c => c.ProvaId)
                .GreaterThan(0)
                .WithMessage("O Id da prova deve ser informado para obter o resumo das questões.");
        }
    }
}