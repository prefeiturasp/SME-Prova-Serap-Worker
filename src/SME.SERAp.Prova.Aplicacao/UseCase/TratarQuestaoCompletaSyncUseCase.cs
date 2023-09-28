using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarQuestaoCompletaSyncUseCase : ITratarQuestaoCompletaSyncUseCase
    {
        private const int QUANTIDADE_PAGINACAO = 1000;

        private readonly IRepositorioQuestao repositorioQuestao;
        private readonly IMediator mediator;

        public TratarQuestaoCompletaSyncUseCase(IRepositorioQuestao repositorioQuestao, IMediator mediator)
        {
            this.repositorioQuestao = repositorioQuestao;
            this.mediator = mediator;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var pagina = 1;
            IEnumerable<QuestaoAtualizada> questoesAtualizadas;
            do
            {
                questoesAtualizadas = await repositorioQuestao.ObterQuestoesAtualizadas(pagina, QUANTIDADE_PAGINACAO);
                if (questoesAtualizadas != null && questoesAtualizadas.Any())
                {
                    foreach (var questaoAtualizada in questoesAtualizadas)
                    {
                        await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.QuestaoCompletaTratar, questaoAtualizada));
                    }
                }

                pagina++;
            }
            while (questoesAtualizadas.Any());

            return true;
        }
    }
}
