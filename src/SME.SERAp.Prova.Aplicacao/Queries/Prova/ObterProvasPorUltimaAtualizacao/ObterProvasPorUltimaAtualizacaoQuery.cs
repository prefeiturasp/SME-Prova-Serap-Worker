using FluentValidation;
using MediatR;
using SME.SERAp.Prova.Infra.Dtos;
using System;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterProvasPorUltimaAtualizacaoQuery : IRequest<IEnumerable<ProvaAtualizadaDto>>
    {
        public ObterProvasPorUltimaAtualizacaoQuery(DateTime dataBase)
        {
            DataBase = dataBase;
        }

        public DateTime DataBase { get; }
    }

    public class ObterProvasPorUltimaAtualizacaoQueryValidator : AbstractValidator<ObterProvasPorUltimaAtualizacaoQuery>
    {
        public ObterProvasPorUltimaAtualizacaoQueryValidator()
        {
            RuleFor(x => x.DataBase).NotEmpty().WithMessage("Informa da data base para obter as provas atualizadas");
        }

    }
}
