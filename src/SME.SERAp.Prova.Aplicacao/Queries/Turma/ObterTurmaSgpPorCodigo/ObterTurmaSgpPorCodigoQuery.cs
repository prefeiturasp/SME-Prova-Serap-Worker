using FluentValidation;
using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmaSgpPorCodigoQuery : IRequest<TurmaSgpDto>
    {
        public ObterTurmaSgpPorCodigoQuery(string codigoTurma)
        {
            CodigoTurma = codigoTurma;
        }

        public string CodigoTurma { get; set; }

        public class ObterTurmaSgpPorCodigoQueryValidator : AbstractValidator<ObterTurmaSgpPorCodigoQuery>
        {
            public ObterTurmaSgpPorCodigoQueryValidator()
            {
                RuleFor(c => c.CodigoTurma)
                    .NotEmpty()
                    .WithMessage("O código da turma no sgp deve ser informado.");
            }
        }

    }
}
