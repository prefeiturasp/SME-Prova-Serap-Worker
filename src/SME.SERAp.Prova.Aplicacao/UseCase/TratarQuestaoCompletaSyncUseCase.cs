using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarQuestaoCompletaSyncUseCase : ITratarQuestaoCompletaSyncUseCase
    {

        private readonly IRepositorioQuestao repositorioQuestao;
        private readonly IMediator mediator;

        public TratarQuestaoCompletaSyncUseCase(IRepositorioQuestao repositorioQuestao, IMediator mediator)
        {
            this.repositorioQuestao = repositorioQuestao;
            this.mediator = mediator;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var questoesAtualizadas = await repositorioQuestao.ObterQuestoesAtualizadas();
            if (questoesAtualizadas != null && questoesAtualizadas.Any())
            {
                foreach(var questaoAtualizada in questoesAtualizadas)
                {
                    await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.QuestaoCompletaTratar, questaoAtualizada));
                }
            }

            return true;
        }
    }
}
