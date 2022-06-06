using FluentValidation;
using MediatR;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmasSgpPorDreCodigoQuery : IRequest<IEnumerable<TurmaSgpDto>>
    {
        public ObterTurmasSgpPorDreCodigoQuery(string dreCodigo)
        {
            DreCodigo = dreCodigo;
        }

        public string DreCodigo { get; set; }
    }
    public class ObterTurmasSgpPorDreCodigoQueryValidator : AbstractValidator<ObterTurmasSgpPorDreCodigoQuery>
    {
        public ObterTurmasSgpPorDreCodigoQueryValidator()
        {
            RuleFor(c => c.DreCodigo)
                .NotEmpty()
                .WithMessage("O código da Ue no sgp deve ser informado.");
        }
    }
}
