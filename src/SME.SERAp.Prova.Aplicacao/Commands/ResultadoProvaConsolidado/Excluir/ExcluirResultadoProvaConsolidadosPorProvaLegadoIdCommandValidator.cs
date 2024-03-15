using FluentValidation;
using SME.SERAp.Prova.Aplicacao.Commands.ConsolidarProvaRespostaAdesaoManual;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Aplicacao.Commands.ResultadoProvaConsolidado.Excluir
{
    public class ExcluirResultadoProvaConsolidadosPorProvaLegadoIdCommandValidator : AbstractValidator<ExcluirResultadoProvaConsolidadosPorProvaLegadoIdCommand>
    {
        public ExcluirResultadoProvaConsolidadosPorProvaLegadoIdCommandValidator()
        {
            RuleFor(x => x.ProvaLegadoId)
                .NotEmpty()
                .WithMessage("Código da prova é obrigatório");
        }
    }
}

