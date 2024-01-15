using System.Collections.Generic;
using FluentValidation;
using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterIdsQuestoesPorProvaIdCadernoQuery : IRequest<IEnumerable<long>>
    {
        public ObterIdsQuestoesPorProvaIdCadernoQuery(long provaId, string caderno)
        {
            ProvaId = provaId;
            Caderno = caderno;
        }

        public long ProvaId { get; }
        public string Caderno { get; }
    }

    public class ObterIdsQuestoesPorProvaIdCadernoQueryValidator : AbstractValidator<ObterIdsQuestoesPorProvaIdCadernoQuery>
    {
        public ObterIdsQuestoesPorProvaIdCadernoQueryValidator()
        {
            RuleFor(c => c.ProvaId)
                .GreaterThan(0)
                .WithMessage("O Id da prova deve ser informado para obter os ids das questões.");

            RuleFor(c => c.Caderno)
                .NotNull()
                .NotEmpty()
                .WithMessage("O caderno deve ser informado para obter os ids das questões.");
        }
    }
}