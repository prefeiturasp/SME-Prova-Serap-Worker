using FluentValidation;
using MediatR;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmasSgpPorUeCodigoEAnoLetivoQuery : IRequest<IEnumerable<TurmaSgpDto>>
    {
        public ObterTurmasSgpPorUeCodigoEAnoLetivoQuery(string ueCodigo, int anoLetivo, bool historica)
        {
            UeCodigo = ueCodigo;
            AnoLetivo = anoLetivo;
            Historica = historica;
        }

        public string UeCodigo { get; set; }
        public int AnoLetivo { get; set; }
        public bool Historica { get; set; }
    }

    public class ObterTurmasSgpPorUeCodigoEAnoLetivoQueryValidator : AbstractValidator<ObterTurmasSgpPorUeCodigoEAnoLetivoQuery>
    {
        public ObterTurmasSgpPorUeCodigoEAnoLetivoQueryValidator()
        {
            RuleFor(c => c.UeCodigo)
                .NotEmpty()
                .NotNull()
                .WithMessage("O Id da Ue no sgp deve ser informado.");

            RuleFor(c => c.AnoLetivo)
                .GreaterThan(0)
                .WithMessage("O ano letivo deve ser informado.");
        }
    }
}
