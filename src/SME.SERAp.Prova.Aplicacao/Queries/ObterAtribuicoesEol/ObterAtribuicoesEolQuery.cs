using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAtribuicoesEolQuery : IRequest<IEnumerable<TurmaAtribuicaoEolDto>>
    {
        public ObterAtribuicoesEolQuery(string codigoRf, long? turmaCodigo = null, int? anoLetivo = null)
        {
            CodigoRf = codigoRf;
            TurmaCodigo = turmaCodigo;
            AnoLetivo = anoLetivo;
        }

        public string CodigoRf { get; }
        public long? TurmaCodigo { get; }
        public int? AnoLetivo { get; }        
    }

    public class ObterAtribuicoesEolQueryValidator : AbstractValidator<ObterAtribuicoesEolQuery>
    {
        public ObterAtribuicoesEolQueryValidator()
        {
            RuleFor(c => c.CodigoRf)
                .NotEmpty()
                .NotNull()
                .WithMessage("O código RF deve ser informado para obter as atribuições no EOL.");

            RuleFor(c => c.TurmaCodigo)
                .GreaterThan(0)
                .When(c => c.TurmaCodigo != null)
                .WithMessage("Um código da turma válido deve ser informado para obter as atribuições no EOL.");
            
            RuleFor(c => c.AnoLetivo)
                .GreaterThan(0)
                .When(c => c.AnoLetivo != null)
                .WithMessage("Um ano letivo válido deve ser informado para obter as atribuições no EOL.");            
        }
    }
}