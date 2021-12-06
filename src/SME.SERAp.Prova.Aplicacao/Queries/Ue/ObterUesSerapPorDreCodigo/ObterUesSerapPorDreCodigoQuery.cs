using FluentValidation;
using MediatR;
using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUesSerapPorDreCodigoQuery : IRequest<IEnumerable<Ue>>
    {
        public ObterUesSerapPorDreCodigoQuery(string dreCodigo)
        {
            DreCodigo = dreCodigo;
        }

        public string DreCodigo { get; set; }
    }
    public class ObterUesSerapPorDreCodigoQueryValidator : AbstractValidator<ObterUesSerapPorDreCodigoQuery>
    {
        public ObterUesSerapPorDreCodigoQueryValidator()
        {
            RuleFor(c => c.DreCodigo)
                .NotEmpty()
                .WithMessage("O código da dre deve ser informado para obter as ues.");
        }
    }
}
