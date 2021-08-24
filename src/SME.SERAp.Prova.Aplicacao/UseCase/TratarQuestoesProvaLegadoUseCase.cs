using System;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarQuestoesProvaLegadoUseCase : ITratarQuestoesProvaLegadoUseCase
    {
        private readonly IMediator mediator;

        public TratarQuestoesProvaLegadoUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var questao = mensagemRabbit.ObterObjetoMensagem<QuestoesPorProvaIdDto>();

            var prova = await mediator.Send(new ObterProvaDetalhesPorIdQuery(questao.ProvaLegadoId));

            if (prova == null)
                throw new Exception($"Prova {questao.ProvaLegadoId} não localizada!");

            var novaQuestao = new Questao(
                questao.Ordem,
                questao.Questao,
                questao.Enunciado,
                questao.ProvaLegadoId,
                questao.QuestaoId,
                prova.Id
            );

            await mediator.Send(new QuestaoParaIncluirCommand(novaQuestao));
            
            var buscarPorProvaIdEQuestaoIdDto =
                new BuscarPorProvaIdEQuestaoIdDto(questao.ProvaLegadoId, questao.QuestaoId);
            
            await mediator.Send(
                new PublicaFilaRabbitCommand(RotasRabbit.AlternativaSync, buscarPorProvaIdEQuestaoIdDto));

            return true;
        }
    }
}