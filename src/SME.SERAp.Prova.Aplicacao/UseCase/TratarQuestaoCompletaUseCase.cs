using MediatR;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarQuestaoCompletaUseCase : ITratarQuestaoCompletaUseCase
    {
        private readonly IMediator mediator;

        public TratarQuestaoCompletaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var questaoAtualizada = mensagemRabbit.ObterObjetoMensagem<QuestaoAtualizada>();

            var questaoCompleta = await mediator.Send(new MontarQuestaoCompletaPorIdQuery(questaoAtualizada.Id));

            if (questaoCompleta == null || questaoCompleta.Id <= 0) 
                return true;
            
            if (questaoCompleta.QuantidadeAlternativas != questaoCompleta.Alternativas.Count())
                throw new NegocioException($"Total de alternativas diferente do informado na questão {questaoAtualizada.Id}");

            var json = JsonSerializer.Serialize(questaoCompleta, new JsonSerializerOptions
                { IgnoreNullValues = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            await mediator.Send(new QuestaoCompletaIncluirCommand(questaoAtualizada.Id, questaoCompleta.QuestaoLegadoId, json, questaoAtualizada.UltimaAtualizacao));
            await mediator.Send(new RemoverQuestaoCacheCommand(questaoAtualizada.Id, questaoCompleta.QuestaoLegadoId));

            return true;
        }
    }
}
