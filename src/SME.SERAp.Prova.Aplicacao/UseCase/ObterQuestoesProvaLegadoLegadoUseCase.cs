using System;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterQuestoesProvaLegadoLegadoUseCase : IObterQuestoesProvaLegadoLegadoUseCase
    {
        
        private readonly IMediator mediator;

        public ObterQuestoesProvaLegadoLegadoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator)); 
        }

        
        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var provaId = long.Parse(mensagemRabbit.Mensagem.ToString());
           
            var questoesIds = await mediator.Send(new ObterQuestoesPorProvaIdQuery(provaId));
            
            foreach (var id in questoesIds)
            {
                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.QuestaoTratar, new DetalheQuestaoDto(id , provaId)));
            }
            
            return true;
        }
    }
}