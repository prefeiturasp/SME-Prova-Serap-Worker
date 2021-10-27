using FluentValidation;
using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.Dtos;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TrataSincronizacaoInstitucionalTurmaCommand : IRequest<long>
    {
        public TrataSincronizacaoInstitucionalTurmaCommand(TurmaSgpDto turmaSgp, Turma turmaSerap)
        {
            TurmaSgp = turmaSgp;
            TurmaSerap = turmaSerap;
        }

        public TurmaSgpDto TurmaSgp { get; set; }
        public Turma TurmaSerap { get; set; }
    }
    public class TrataSincronizacaoInstitucionalTurmaCommandValidator : AbstractValidator<TrataSincronizacaoInstitucionalTurmaCommand>
    {
        public TrataSincronizacaoInstitucionalTurmaCommandValidator()
        {
            RuleFor(c => c.TurmaSgp)
                .NotEmpty()
                .WithMessage("A Turma do Sgp deve ser informada para sincronização.");
        }
    }
}
