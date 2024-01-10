using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAmostrasUtilizarTaiCacheQuery : IRequest<IEnumerable<ItemAmostraTaiDto>>
    {
        public ObterAmostrasUtilizarTaiCacheQuery(long provaLegadoId)
        {
            ProvaLegadoId = provaLegadoId;
        }

        public long ProvaLegadoId { get; }
    }

    public class ObterAmostrasUtilizarTaiCacheQueryValidator : AbstractValidator<ObterAmostrasUtilizarTaiCacheQuery>
    {
        public ObterAmostrasUtilizarTaiCacheQueryValidator()
        {
            RuleFor(c => c.ProvaLegadoId)
                .GreaterThan(0)
                .WithMessage("O Id da prova do legado deve ser informado para obter os itens da amostra da prova.");
        }
    }
}