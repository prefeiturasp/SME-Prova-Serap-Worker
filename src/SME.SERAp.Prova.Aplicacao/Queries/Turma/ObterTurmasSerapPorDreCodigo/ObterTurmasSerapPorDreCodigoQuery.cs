using FluentValidation;
using MediatR;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmasSerapPorDreCodigoQuery : IRequest<IEnumerable<TurmaSgpDto>>
    {
        public ObterTurmasSerapPorDreCodigoQuery(string dreCodigo)
        {
            DreCodigo = dreCodigo;
        }

        public string DreCodigo { get; set; }
    }
    public class ObterTurmasSerapPorDreCodigoQueryValidator : AbstractValidator<ObterTurmasSerapPorDreCodigoQuery>
    {
        public ObterTurmasSerapPorDreCodigoQueryValidator()
        {
            RuleFor(c => c.DreCodigo)
                .NotEmpty()
                .WithMessage("O código da Dre no sgp deve ser informado.");
        }
    }
}
