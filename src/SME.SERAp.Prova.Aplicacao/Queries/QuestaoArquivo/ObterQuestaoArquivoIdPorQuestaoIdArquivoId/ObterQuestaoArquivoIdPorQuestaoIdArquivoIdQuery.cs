using FluentValidation;
using MediatR;

namespace SME.SERAp.Prova.Aplicacao.Queries
{
    public class ObterQuestaoArquivoIdPorQuestaoIdArquivoIdQuery : IRequest<long>
    {
        public ObterQuestaoArquivoIdPorQuestaoIdArquivoIdQuery(long questaoId, long arquivoId)
        {
            QuestaoId = questaoId;
            ArquivoId = arquivoId;
        }

        public long QuestaoId { get; }
        public long ArquivoId { get; }
    }

    public class ObterQuestaoArquivoIdPorQuestaoIdArquivoIdQueryValidator : AbstractValidator<ObterQuestaoArquivoIdPorQuestaoIdArquivoIdQuery>
    {
        public ObterQuestaoArquivoIdPorQuestaoIdArquivoIdQueryValidator()
        {
            RuleFor(c => c.QuestaoId)
                .GreaterThan(0)
                .WithMessage("O id da questão deve ser informado para obter o id do arquivo da questão.");

            RuleFor(c => c.ArquivoId)
                .GreaterThan(0)
                .WithMessage("O id do arquivo deve ser informado para obter o id do arquivo da questão.");
        }
    }
}