﻿using System;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarAlternativaProvaLegadoLegadoUseCase : ITratarAlternativaProvaLegadoLegadoUseCase
    {
        private readonly IMediator mediator;

        public TratarAlternativaProvaLegadoLegadoUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var alternativa = mensagemRabbit.ObterObjetoMensagem<AlternativasProvaIdDto>();

            var questao = await mediator.Send(new ObterQuestaoPorProvaLegadoQuery(alternativa.QuestaoLegadoId));

            if (questao == null)
                throw new Exception(
                    $"A questao {alternativa.QuestaoLegadoId} da prova {alternativa.ProvaLegadoId} não localizada!");


            
            
            var alternativas = new Alternativas(
                alternativa.ProvaLegadoId,
                alternativa.QuestaoLegadoId,
                alternativa.AlternativaLegadoId,
                alternativa.Ordem,
                alternativa.Alternativa,
                alternativa.Descricao,
                alternativa.Correta,
                questao.Id
            );
            await mediator.Send(new AlternativasParaIncluirCommand(alternativas));

            return true;
        }
    }
}