using FluentValidation;
using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class SalvarCacheJsonCommand : IRequest<bool>
    {
        public SalvarCacheJsonCommand(string nomeCache, string json, int? minutosParaExpirar)
        {
            NomeCache = nomeCache;
            Json = json;
            MinutosParaExpirar = minutosParaExpirar;
        }

        public string NomeCache { get; }
        public string Json { get; }
        public int? MinutosParaExpirar { get; }        
    }

    public class SalvarCacheJsonCommandValidator : AbstractValidator<SalvarCacheJsonCommand>
    {
        public SalvarCacheJsonCommandValidator()
        {
            RuleFor(x => x.NomeCache)
                .NotEmpty()
                .WithMessage("Informe o nome da chave para salvar o cache");

            RuleFor(x => x.Json)
                .NotEmpty()
                .NotNull()
                .WithMessage("Informe o JSON da chave para salvar o cache");

            RuleFor(x => x.MinutosParaExpirar)
                .GreaterThan(0).When(x => x.MinutosParaExpirar != null)
                .WithMessage("Minutos para expirar deve ser maior que 0.");            
        }
    }
}