using FluentValidation;
using SME.SERAp.Prova.Aplicacao.Commands.ResultadoProvaConsolidado.Incluir;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Aplicacao.Queries.ObterAlunosResultadoProvaAdesao
{
    public class ObterAlunosResultadoProvaAdesaoQueryValidator : AbstractValidator<ObterAlunosResultadoProvaAdesaoQuery>
    {
        public ObterAlunosResultadoProvaAdesaoQueryValidator()
        {
            RuleFor(x => x.ProvaId)
                .NotEmpty()
                    .WithMessage("Código da prova é obrigatório");
        }
    }
}