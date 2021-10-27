using FluentValidation;
using MediatR;
using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUesSgpPorDreCodigoQuery : IRequest<IEnumerable<Ue>>
    {
        public ObterUesSgpPorDreCodigoQuery(string dreCodigo)
        {
            DreCodigo = dreCodigo;
        }

        public string DreCodigo { get; set; }
    }
    public class ObterUesSgpPorDreCodigoQueryValidator : AbstractValidator<ObterUesSgpPorDreCodigoQuery>
    {
        public ObterUesSgpPorDreCodigoQueryValidator()
        {
            RuleFor(c => c.DreCodigo)
                .NotEmpty()
                .WithMessage("O código da dre deve ser informado para obter as ues.");
        }
    }
}
