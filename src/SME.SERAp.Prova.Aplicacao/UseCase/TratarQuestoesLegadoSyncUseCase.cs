using System;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarQuestoesLegadoSyncUseCase : ITratarQuestoesLegadoSyncUseCase
    {
        
        private readonly IMediator mediator;

        public TratarQuestoesLegadoSyncUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator)); 
        }

        
        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var provaId = long.Parse(mensagemRabbit.Mensagem.ToString());
           
            var questoes = await mediator.Send(new ObterQuestoesPorProvaIdQuery(provaId));
            
            foreach (var questao in questoes)
            {
                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.QuestaoTratar, new DetalheQuestaoDto(questao.Id, questao.Ordem, provaId)));
            }
            
            return true;
        }
    }
}