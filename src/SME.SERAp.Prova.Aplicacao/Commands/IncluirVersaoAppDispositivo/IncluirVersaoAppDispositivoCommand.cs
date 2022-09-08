using FluentValidation;
using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
   public class IncluirVersaoAppDispositivoCommand : IRequest<bool>
    {
        public IncluirVersaoAppDispositivoCommand(VersaoAppDispositivo versaoAppDispositivo)
        {
            VersaoAppDispositivo = versaoAppDispositivo;
        }

        public VersaoAppDispositivo VersaoAppDispositivo { get; set; }
    }

    public class IncluirVersaoAppDispositivoCommandValidator : AbstractValidator<IncluirVersaoAppDispositivoCommand>
    {
        public IncluirVersaoAppDispositivoCommandValidator()
        {
            RuleFor(c => c.VersaoAppDispositivo.VersaoDescricao)
               .NotEmpty()
               .WithMessage("A Descrição da versão deve ser informado.");            

            RuleFor(c => c.VersaoAppDispositivo.AtualizadoEm)
                .NotEmpty()
                .WithMessage("A data e hora em que o dispositivo foi atualizado deve ser informado.");
        }
    }
}
