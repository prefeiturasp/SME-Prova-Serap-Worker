using FluentValidation;
using MediatR;
using SME.SERAp.Prova.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmasSgpPorUeIdQuery : IRequest<IEnumerable<TurmaSgpDto>>
    {
        public ObterTurmasSgpPorUeIdQuery(long ueId)
        {
            UeId = ueId;
        }

        public long UeId { get; set; }
    }
    public class ObterTurmasSgpPorUeIdQueryValidator : AbstractValidator<ObterTurmasSgpPorUeIdQuery>
    {
        public ObterTurmasSgpPorUeIdQueryValidator()
        {
            RuleFor(c => c.UeId)
                .NotEmpty()
                .WithMessage("O id da Ue no sgp deve ser informado.");
        }
    }
}
