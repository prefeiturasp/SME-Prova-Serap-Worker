﻿using System;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarAlternativaLegadoLegadoUseCase : ITratarAlternativaLegadoUseCase
    {
        private readonly IMediator mediator;

        public TratarAlternativaLegadoLegadoUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var detalheAlternativaDto = mensagemRabbit.ObterObjetoMensagem<DetalheAlternativaDto>();

            var alternativa =
                await mediator.Send(
                    new ObterAlternativaDetalheLegadoPorIdQuery(
                        detalheAlternativaDto.QuestaoId, detalheAlternativaDto.AlternativaId));

            if (alternativa == null)
                throw new Exception(
                    $"A Alternativa {alternativa.AlternativaLegadoId} não localizada!");

            var questao = await mediator.Send(new ObterQuestaoPorProvaLegadoQuery(detalheAlternativaDto.QuestaoId));

            if (questao == null)
                throw new Exception(
                    $"A Alternativa {alternativa.AlternativaLegadoId} não localizada!");

            var alternativas = new Alternativa(
                alternativa.Ordem,
                alternativa.Numeracao,
                alternativa.Descricao,
                questao.Id
            );

            await mediator.Send(new AlternativaIncluirCommand(alternativas));

            return true;
        }
    }
}