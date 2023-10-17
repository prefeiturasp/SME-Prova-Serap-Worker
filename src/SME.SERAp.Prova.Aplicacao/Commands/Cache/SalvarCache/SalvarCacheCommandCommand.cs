using FluentValidation;
using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class SalvarCacheCommandCommand : IRequest<bool>
    {
        public SalvarCacheCommandCommand(string nomeCache, object valor)
        {
            NomeCache = nomeCache;
            Valor = valor;
        }

        public string NomeCache { get; }
        public object Valor { get; }
    }

    public class SalvarCacheCommandCommandValidator : AbstractValidator<SalvarCacheCommandCommand>
    {
        public SalvarCacheCommandCommandValidator()
        {
            RuleFor(x => x.NomeCache)
                .NotEmpty()
                .WithMessage("Informe o nome da chave para salvar o cache");

            RuleFor(x => x.Valor)
                .NotEmpty()
                .WithMessage("Informe o valor da chave para salvar o cache");
        }
    }
}
