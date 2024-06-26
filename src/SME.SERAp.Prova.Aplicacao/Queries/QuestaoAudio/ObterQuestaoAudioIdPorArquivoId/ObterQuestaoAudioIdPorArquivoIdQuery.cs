﻿using FluentValidation;
using MediatR;

namespace SME.SERAp.Prova.Aplicacao.Queries
{
    public class ObterQuestaoAudioIdPorArquivoIdQuery : IRequest<long>
    {
        public ObterQuestaoAudioIdPorArquivoIdQuery(long arquivoId)
        {
            ArquivoId = arquivoId;
        }

        public long ArquivoId { get; }
    }

    public class ObterQuestaoAudioIdPorArquivoIdQueryValidator : AbstractValidator<ObterQuestaoAudioIdPorArquivoIdQuery>
    {
        public ObterQuestaoAudioIdPorArquivoIdQueryValidator()
        {
            RuleFor(c => c.ArquivoId)
                .GreaterThan(0)
                .WithMessage("O id do arquivo deve ser informado para obter o id do áudio da questão.");
        }
    }
}