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
            var ultimaAtualizacao = await mediator.Send(new ObterUltimoExecucaoControleTipoPorTipoQuery(ExecucaoControleTipo.ProvaLegadoSincronizacao));
            
            var questoes = await mediator.Send(new ObterQuestoesPorProvaIdQuery(provaId));
            
            foreach (var questao in questoes)
            {
                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.QuestaoTratar, questao));
            }

            await mediator.Send(new ExecucaoControleAtualizarCommand(ultimaAtualizacao));
            return true;
        }
    }
}