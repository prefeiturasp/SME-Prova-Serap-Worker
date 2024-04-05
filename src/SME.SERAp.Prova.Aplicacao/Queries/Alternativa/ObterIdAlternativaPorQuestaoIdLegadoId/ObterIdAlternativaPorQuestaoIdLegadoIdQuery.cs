using FluentValidation;
using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterIdAlternativaPorQuestaoIdLegadoIdQuery : IRequest<long>
    {
        public ObterIdAlternativaPorQuestaoIdLegadoIdQuery(long questaoId, long alternativaLegadoId)
        {
            QuestaoId = questaoId;
            AlternativaLegadoId = alternativaLegadoId;
        }

        public long QuestaoId { get; }
        public long AlternativaLegadoId { get; }
    }

    public class ObterIdAlternativaPorQuestaoIdLegadoIdQueryValidator : AbstractValidator<ObterIdAlternativaPorQuestaoIdLegadoIdQuery>
    {
        public ObterIdAlternativaPorQuestaoIdLegadoIdQueryValidator()
        {
            RuleFor(x => x.QuestaoId)
                .GreaterThan(0)
                .WithMessage(" Informe o id da questão para obter o id da alternativa");

            RuleFor(x => x.AlternativaLegadoId)
                .GreaterThan(0)
                .WithMessage("  Informe o id legado da alternativa para obter o id da alternativa");
        }
    }
}
