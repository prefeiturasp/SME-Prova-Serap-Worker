using FluentValidation;
using SME.SERAp.Prova.Aplicacao.Commands.ConsolidarProvaRespostaAdesaoManual;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Aplicacao.Commands.ResultadoProvaConsolidado.Incluir
{
    public class InserirResultadoProvaConsolidadoCommandValidador : AbstractValidator<InserirResultadoProvaConsolidadoCommand>
    {
        public InserirResultadoProvaConsolidadoCommandValidador()
        {
            RuleFor(x => x.ResultadoProvaConsolidado)
                .NotNull().NotEmpty()
                .WithMessage("O Objeto ResultadoProvaConsolidado é obrigatório");
        }
    }
}