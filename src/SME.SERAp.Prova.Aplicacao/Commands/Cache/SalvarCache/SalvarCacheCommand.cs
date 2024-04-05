using FluentValidation;
using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class SalvarCacheCommand : IRequest<bool>
    {
        public SalvarCacheCommand(string nomeCache, object valor, int? minutosParaExpirar = null)
        {
            NomeCache = nomeCache;
            Valor = valor;
            MinutosParaExpirar = minutosParaExpirar;
        }

        public string NomeCache { get; }
        public object Valor { get; }
        public int? MinutosParaExpirar { get; }
    }

    public class SalvarCacheCommandValidator : AbstractValidator<SalvarCacheCommand>
    {
        public SalvarCacheCommandValidator()
        {
            RuleFor(x => x.NomeCache)
                .NotEmpty()
                .WithMessage("Informe o nome da chave para salvar o cache");

            RuleFor(x => x.Valor)
                .NotEmpty()
                .WithMessage("Informe o valor da chave para salvar o cache");

            RuleFor(x => x.MinutosParaExpirar)
                .GreaterThan(0).When(x => x.MinutosParaExpirar != null)
                .WithMessage("Minutos para expirar deve ser maior que 0.");
        }
    }
}
