using MediatR;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.UseCase
{
    public class TratarQuestaoCompletaProvaUseCase : AbstractUseCase, ITratarQuestaoCompletaProvaUseCase
    {
        private const int QUANTIDADE_PAGINACAO = 5000;
        public TratarQuestaoCompletaProvaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var provaAtualizada = mensagemRabbit.ObterObjetoMensagem<ProvaAtualizadaDto>();

            var pagina = 1;
            IEnumerable<QuestaoAtualizada> questoesAtualizadas;
            do
            {
                questoesAtualizadas = await mediator.Send(new ObterQuestoesAtualizadasQuery(provaAtualizada.ProvaId, pagina, QUANTIDADE_PAGINACAO));
                if (questoesAtualizadas != null && questoesAtualizadas.Any())
                {
                    foreach (var questaoAtualizada in questoesAtualizadas)
                    {
                        if (provaAtualizada.UltimaAtualizacao != questaoAtualizada.UltimaAtualizacaoQuestao.GetValueOrDefault())
                        {
                            questaoAtualizada.UltimaAtualizacao = provaAtualizada.UltimaAtualizacao;
                            await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.QuestaoCompletaTratar, questaoAtualizada));
                        }
                    }
                }

                pagina++;
            }
            while (questoesAtualizadas.Any());

            return true;
        }
    }
}
