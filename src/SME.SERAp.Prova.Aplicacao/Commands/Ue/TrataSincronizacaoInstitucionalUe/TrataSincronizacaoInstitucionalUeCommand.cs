using FluentValidation;
using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TrataSincronizacaoInstitucionalUeCommand : IRequest<long>
    {
        public TrataSincronizacaoInstitucionalUeCommand(Ue ueSgp, Ue ueSerap)
        {
            UeSgp = ueSgp;
            UeSerap = ueSerap;
        }

        public Ue UeSgp { get; set; }
        public Ue UeSerap { get; set; }
    }
    public class TrataSincronizacaoInstitucionalUeCommandValidator : AbstractValidator<TrataSincronizacaoInstitucionalUeCommand>
    {
        public TrataSincronizacaoInstitucionalUeCommandValidator()
        {
            RuleFor(c => c.UeSgp)
                .NotEmpty()
                .WithMessage("A UeSgp deve ser informada.");
        }
    }
}
