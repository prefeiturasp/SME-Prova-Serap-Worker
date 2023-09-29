using FluentValidation;
using MediatR;

namespace SME.SERAp.Prova.Aplicacao.Queries
{
    public class ObterIdArquivoPorCaminhoQuery : IRequest<long>
    {
        public ObterIdArquivoPorCaminhoQuery(string caminho)
        {
            Caminho = caminho;
        }

        public string Caminho { get; }
    }

    public class ObterIdArquivoPorCaminhoQueryValidator : AbstractValidator<ObterIdArquivoPorCaminhoQuery>
    {
        public ObterIdArquivoPorCaminhoQueryValidator()
        {
            RuleFor(c => c.Caminho)
                .NotNull()
                .NotEmpty()
                .WithMessage("O caminho deve ser informado para obter o id do arquivo.");
        }
    }
}