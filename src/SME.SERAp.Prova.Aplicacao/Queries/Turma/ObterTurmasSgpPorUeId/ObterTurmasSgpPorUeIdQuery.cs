using FluentValidation;
using MediatR;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmasSgpPorUeIdQuery : IRequest<IEnumerable<TurmaSgpDto>>
    {
        public ObterTurmasSgpPorUeIdQuery(string ueCodigo)
        {
            UeCodigo = ueCodigo;
        }

        public string UeCodigo { get; set; }
    }
    public class ObterTurmasSgpPorUeIdQueryValidator : AbstractValidator<ObterTurmasSgpPorUeIdQuery>
    {
        public ObterTurmasSgpPorUeIdQueryValidator()
        {
            RuleFor(c => c.UeCodigo)
                .NotEmpty()
                .WithMessage("O código da Ue no sgp deve ser informado.");
        }
    }
}
