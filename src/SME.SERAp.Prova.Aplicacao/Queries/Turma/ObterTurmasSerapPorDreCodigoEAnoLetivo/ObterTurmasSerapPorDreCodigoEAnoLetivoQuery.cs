using FluentValidation;
using MediatR;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmasSerapPorDreCodigoEAnoLetivoQuery : IRequest<IEnumerable<TurmaSgpDto>>
    {
        public ObterTurmasSerapPorDreCodigoEAnoLetivoQuery(string dreCodigo, long anoLetivo)
        {
            DreCodigo = dreCodigo;
            AnoLetivo = anoLetivo;
        }

        public string DreCodigo { get; set; }
        public long AnoLetivo { get; set; }

    }

    public class ObterTurmasSerapPorDreCodigoEAnoLetivoQueryValidator : AbstractValidator<ObterTurmasSerapPorDreCodigoEAnoLetivoQuery>
    {
        public ObterTurmasSerapPorDreCodigoEAnoLetivoQueryValidator()
        {
            RuleFor(c => c.DreCodigo)
                .NotEmpty()
                .WithMessage("O código da Dre no sgp deve ser informado.");
            RuleFor(c => c.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado.");
        }
    }
}
