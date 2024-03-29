﻿using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarOrdemQuestaoAlunoProvaTaiUseCase : AbstractUseCase, ITratarOrdemQuestaoAlunoProvaTaiUseCase
    {
        public TratarOrdemQuestaoAlunoProvaTaiUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var ordemQuestaoTai = mensagemRabbit.ObterObjetoMensagem<OrdemQuestaoTaiDto>();
            if (ordemQuestaoTai == null)
                throw new NegocioException($"É preciso informar os dados de ordem da questão.");

            var questao = await mediator.Send(new ObterQuestaoPorIdQuery(ordemQuestaoTai.QuestaoId));
            await Validacoes(ordemQuestaoTai, questao);

            questao.Ordem = ordemQuestaoTai.Ordem;
            await mediator.Send(new QuestaoParaAtualizarCommand(questao));

            return true;
        }

        private async Task Validacoes(OrdemQuestaoTaiDto ordemQuestaoTai, Questao questao)
        {
            if (questao == null)
                throw new NegocioException($"Questão não encontrada.");

            var prova = await mediator.Send(new ObterProvaPorIdQuery(questao.ProvaId));
            if (prova == null)
                throw new NegocioException($"Prova não encontrada.");

            if (!prova.FormatoTai)
                throw new NegocioException($"Prova não é formato TAI.");
        }
    }
}
