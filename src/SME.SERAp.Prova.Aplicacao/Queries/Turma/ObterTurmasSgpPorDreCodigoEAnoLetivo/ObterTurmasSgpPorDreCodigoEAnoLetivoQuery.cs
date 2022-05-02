using FluentValidation;
using MediatR;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmasSgpPorDreCodigoEAnoLetivoQuery : IRequest<IEnumerable<TurmaSgpDto>>
    {
        public ObterTurmasSgpPorDreCodigoEAnoLetivoQuery(string dreCodigo, long anoLetivo, bool historica)
        {
            DreCodigo = dreCodigo;
            AnoLetivo = anoLetivo;
            Historica = historica;
        }

        public string DreCodigo { get; set; }
        public long AnoLetivo { get; set; }
        public bool Historica { get; set; }

    }

    public class ObterTurmasSgpPorDreCodigoEAnoLetivoQueryValidator : AbstractValidator<ObterTurmasSgpPorDreCodigoEAnoLetivoQuery>
    {
        public ObterTurmasSgpPorDreCodigoEAnoLetivoQueryValidator()
        {
            RuleFor(c => c.DreCodigo)
                .NotEmpty()
                .WithMessage("O código da Ue no sgp deve ser informado.");

            RuleFor(c => c.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado.");

            RuleFor(c => c.Historica)
                .NotNull()
                .WithMessage("Opção Historica deve ser informado.");
        }
    }
}
