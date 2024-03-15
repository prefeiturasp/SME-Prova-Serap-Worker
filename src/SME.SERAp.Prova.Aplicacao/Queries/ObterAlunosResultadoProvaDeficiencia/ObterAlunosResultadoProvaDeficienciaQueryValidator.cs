using FluentValidation;
using SME.SERAp.Prova.Aplicacao.Queries.ObterAlunosResultadoProvaAdesao;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Aplicacao.Queries.ObterAlunosResultadoProvaDeficiencia
{
    public class ObterAlunosResultadoProvaDeficienciaQueryValidator : AbstractValidator<ObterAlunosResultadoProvaDeficienciaQuery>
    {
        public ObterAlunosResultadoProvaDeficienciaQueryValidator()
        {
            RuleFor(x => x.ProvaId)
                .NotEmpty()
                    .WithMessage("Código da prova é obrigatório");
        }
    }
}
