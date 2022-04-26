using FluentValidation;
using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmaPorCodigoUeQuery : IRequest<Turma>
    {
        public ObterTurmaPorCodigoUeQuery(string turmaCodigo)
        {
            TurmaCodigo = turmaCodigo;
        }

        public string TurmaCodigo { get; set; }
    }
    public class ObterTurmaPorCodigoUeQueryValidator : AbstractValidator<ObterTurmaPorCodigoUeQuery>
    {
        public ObterTurmaPorCodigoUeQueryValidator()
        {
            RuleFor(c => c.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado.");
        }
    }
}
