using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Aplicacao.Commands.ConsolidarProvaRespostaAdesaoManual
{
    public class ConsolidarProvaRespostaAdesaoManualCommandHandlerValidator : AbstractValidator<ConsolidarProvaRespostaAdesaoManualCommand>
    {
        public ConsolidarProvaRespostaAdesaoManualCommandHandlerValidator()
        {
            RuleFor(x => x.ProvaId)
                .NotEmpty()
                .WithMessage("Código da prova é obrigatório");
        }
    }
}
