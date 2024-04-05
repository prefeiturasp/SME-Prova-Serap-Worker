using FluentValidation;
using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterCacheQuery : IRequest<object>
    {
        public ObterCacheQuery(string nomeCache)
        {
            NomeCache = nomeCache;
        }

        public string NomeCache { get; }
    }

    public class ObterCacheQueryValidator: AbstractValidator<ObterCacheQuery>
    {
        public ObterCacheQueryValidator()
        {
            RuleFor(x => x.NomeCache)
                .NotEmpty()
                .WithMessage("Informe o nome da chave para obter o cache");
        }
    }
}
