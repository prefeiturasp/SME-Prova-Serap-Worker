using FluentValidation;
using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class SincronizarAlunoCommand : IRequest<long>
    {
        public SincronizarAlunoCommand(AlunoEolDto alunoEol)
        {
            AlunoEol = alunoEol;
        }

        public AlunoEolDto AlunoEol { get; set; }
    }
    public class SincronizarAlunoCommandValidator : AbstractValidator<SincronizarAlunoCommand>
    {
        public SincronizarAlunoCommandValidator()
        {
            RuleFor(c => c.AlunoEol)
                .NotEmpty()
                .WithMessage("O AlunoEol deve ser informado.");
        }
    }
}
