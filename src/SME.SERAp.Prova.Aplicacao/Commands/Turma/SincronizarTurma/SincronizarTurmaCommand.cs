using FluentValidation;
using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class SincronizarTurmaCommand : IRequest<long>
    {
        public SincronizarTurmaCommand(TurmaSgpDto turmaSgp)
        {
            TurmaSgp = turmaSgp;
        }

        public TurmaSgpDto TurmaSgp { get; set; }
    }
    public class SincronizarTurmaCommandValidator : AbstractValidator<SincronizarTurmaCommand>
    {
        public SincronizarTurmaCommandValidator()
        {
            RuleFor(c => c.TurmaSgp)
                .NotEmpty()
                .WithMessage("A Turma do Sgp deve ser informada para sincronização.");
        }
    }
}
