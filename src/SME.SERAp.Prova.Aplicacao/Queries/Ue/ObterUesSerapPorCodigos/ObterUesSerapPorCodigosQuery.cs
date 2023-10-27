using FluentValidation;
using MediatR;
using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUesSerapPorCodigosQuery : IRequest<IEnumerable<Ue>>
    {
        public ObterUesSerapPorCodigosQuery(string[] codigos)
        {
            Codigos = codigos;
        }

        public string[] Codigos { get; }
    }

    public class ObterUesSerapPorCodigosQueryValidator : AbstractValidator<ObterUesSerapPorCodigosQuery>
    {
        public ObterUesSerapPorCodigosQueryValidator()
        {
            RuleFor(x => x.Codigos)
                .NotEmpty()
                .WithMessage("Informe ao menos um código para consultar as ues");
        }
    }
}
