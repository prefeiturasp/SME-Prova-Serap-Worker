using System;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class PropagarCacheQuestoesCompletasLegadoProvaTratarUseCase : AbstractUseCase, IPropagarCacheQuestoesCompletasLegadoProvaTratarUseCase
    {
        public PropagarCacheQuestoesCompletasLegadoProvaTratarUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var mensagem = mensagemRabbit.Mensagem.ToString();
            
            if (string.IsNullOrEmpty(mensagem))
                return false;
            
            var questaoId = long.Parse(mensagem);            
            
            if (questaoId <= 0)
                return false;

            var questaoCompleta = await mediator.Send(new ObterQuestaoCompletaPorQuestaoIdQuery(questaoId));

            if (questaoCompleta == null)
                return false;

            var chaveCache = string.Format(CacheChave.QuestaoCompletaLegado, questaoId);
            var minutosParaUmDia = (int)TimeSpan.FromDays(1).TotalMinutes;
            await mediator.Send(new SalvarCacheJsonCommand(chaveCache, questaoCompleta.Json, minutosParaUmDia));

            return true;
        }
    }
}