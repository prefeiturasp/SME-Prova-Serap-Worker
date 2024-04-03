using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmasPorCodigoDreEProvaSerapQuery : IRequest<IEnumerable<Turma>>
    {
        public ObterTurmasPorCodigoDreEProvaSerapQuery(string codigoDre, long provaSerap)
        {
            CodigoDre = codigoDre;
            ProvaSerap = provaSerap;
        }

        public string CodigoDre { get; }
        public long ProvaSerap { get; }        
    }

    public class ObterTurmasPorCodigoDreEProvaSerapQueryValidator : AbstractValidator<ObterTurmasPorCodigoDreEProvaSerapQuery>
    {
        public ObterTurmasPorCodigoDreEProvaSerapQueryValidator()
        {
            RuleFor(c => c.CodigoDre)
                .NotEmpty()
                .NotNull()
                .WithMessage("O código da DRE deve ser informado para obter as turmas da prova.");

            RuleFor(c => c.ProvaSerap)
                .GreaterThan(0)
                .WithMessage("A prova deve ser informada para obter as turmas.");
        }
    }
}