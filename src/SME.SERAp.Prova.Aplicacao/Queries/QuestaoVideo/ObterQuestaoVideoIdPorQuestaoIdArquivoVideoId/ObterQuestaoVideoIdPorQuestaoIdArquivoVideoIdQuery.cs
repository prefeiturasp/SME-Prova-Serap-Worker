using FluentValidation;
using MediatR;

namespace SME.SERAp.Prova.Aplicacao.Queries
{
    public class ObterQuestaoVideoIdPorQuestaoIdArquivoVideoIdQuery : IRequest<long>
    {
        public ObterQuestaoVideoIdPorQuestaoIdArquivoVideoIdQuery(long questaoId, long arquivoVideoId)
        {
            QuestaoId = questaoId;
            ArquivoVideoId = arquivoVideoId;
        }

        public long QuestaoId { get; }
        public long ArquivoVideoId { get; }
    }

    public class ObterQuestaoVideoIdPorQuestaoIdArquivoVideoIdQueryValidator : AbstractValidator<ObterQuestaoVideoIdPorQuestaoIdArquivoVideoIdQuery>
    {
        public ObterQuestaoVideoIdPorQuestaoIdArquivoVideoIdQueryValidator()
        {
            RuleFor(c => c.QuestaoId)
                .GreaterThan(0)
                .WithMessage("O id da questão deve ser informada para obter o id do vídeo da questão.");

            RuleFor(c => c.ArquivoVideoId)
                .GreaterThan(0)
                .WithMessage("O id do arquivo de vídeo deve ser informado para obter o id do vídeo da questão.");
        }
    }
}