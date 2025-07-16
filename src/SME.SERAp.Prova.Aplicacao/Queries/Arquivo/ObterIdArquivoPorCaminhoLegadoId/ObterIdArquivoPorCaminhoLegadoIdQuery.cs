using FluentValidation;
using MediatR;

namespace SME.SERAp.Prova.Aplicacao.Queries
{
    public class ObterIdArquivoPorCaminhoLegadoIdQuery : IRequest<long>
    {
        public ObterIdArquivoPorCaminhoLegadoIdQuery(string caminho, long legadoId)
        {
            Caminho = caminho;
            LegadoId = legadoId;
        }

        public string Caminho { get; }

        public long LegadoId { get; }
    }

    public class ObterIdArquivoPorCaminhoLegadoIdQueryValidator : AbstractValidator<ObterIdArquivoPorCaminhoLegadoIdQuery>
    {
        public ObterIdArquivoPorCaminhoLegadoIdQueryValidator()
        {
            RuleFor(c => c.Caminho)
                .NotNull()
                .NotEmpty()
                .WithMessage("O caminho deve ser informado para obter o id do arquivo.");

            RuleFor(c => c.LegadoId)
                .NotNull()
                .GreaterThanOrEqualTo(1)
                .WithMessage("O legado id deve ser informado para obter o id do arquivo.");
        }
    }
}