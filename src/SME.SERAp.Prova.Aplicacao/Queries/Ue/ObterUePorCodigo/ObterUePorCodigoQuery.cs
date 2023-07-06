using FluentValidation;
using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUePorCodigoQuery : IRequest<Ue>
    {
        public ObterUePorCodigoQuery(string ueCodigo)
        {
            UeCodigo = ueCodigo;
        }

        public string UeCodigo { get; }
    }
    public class ObterUePorCodigoQueryValidator : AbstractValidator<ObterUePorCodigoQuery>
    {
        public ObterUePorCodigoQueryValidator()
        {
            RuleFor(c => c.UeCodigo)
                .NotEmpty()
                .WithMessage("O código da Ue deve ser informado.");
        }
    }
}
