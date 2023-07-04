using FluentValidation;
using MediatR;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmasSerapPorUeCodigoEAnoLetivoQuery : IRequest<IEnumerable<TurmaSgpDto>>
    {
        public ObterTurmasSerapPorUeCodigoEAnoLetivoQuery(string ueCodigo, int anoLetivo)
        {
            UeCodigo = ueCodigo;
            AnoLetivo = anoLetivo;
        }

        public string UeCodigo { get; set; }
        public int AnoLetivo { get; set; }
    }

    public class ObterTurmasSerapPorUeCodigoEAnoLetivoQueryValidator : AbstractValidator<ObterTurmasSerapPorUeCodigoEAnoLetivoQuery>
    {
        public ObterTurmasSerapPorUeCodigoEAnoLetivoQueryValidator()
        {
            RuleFor(c => c.UeCodigo)
                .NotEmpty()
                .NotNull()
                .WithMessage("O código da Dre no sgp deve ser informado.");
            
            RuleFor(c => c.AnoLetivo)
                .GreaterThan(0)
                .WithMessage("O ano letivo deve ser informado.");
        }
    }
}
