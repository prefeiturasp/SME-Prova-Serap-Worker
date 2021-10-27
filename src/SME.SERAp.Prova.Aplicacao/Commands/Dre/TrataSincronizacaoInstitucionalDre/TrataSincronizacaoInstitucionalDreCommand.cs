using FluentValidation;
using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TrataSincronizacaoInstitucionalDreCommand : IRequest<long>
    {
        public TrataSincronizacaoInstitucionalDreCommand(Dre dreSgp, Dre dreSerap)
        {
            DreSgp = dreSgp;
            DreSerap = dreSerap;
        }

        public Dre DreSgp { get; set; }
        public Dre DreSerap { get; set; }
    }
    public class TrataSincronizacaoInstitucionalDreCommandValidator : AbstractValidator<TrataSincronizacaoInstitucionalDreCommand>
    {
        public TrataSincronizacaoInstitucionalDreCommandValidator()
        {
            RuleFor(c => c.DreSgp)
                .NotEmpty()
                .WithMessage("A DreSgp deve ser informada.");
        }
    }
}
