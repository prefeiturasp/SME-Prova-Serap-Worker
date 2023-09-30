using MediatR;
using SME.SERAp.Prova.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarQuestaoCompletaSyncUseCase : AbstractUseCase, ITratarQuestaoCompletaSyncUseCase
    {
        const int DIAS_ANTERIORES = -3;
        public TratarQuestaoCompletaSyncUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var dataBase = DateTime.Now.AddDays(DIAS_ANTERIORES);
            var provasAtualizadas = await mediator.Send(new ObterProvasPorUltimaAtualizacaoQuery(dataBase));

            if (provasAtualizadas != null && provasAtualizadas.Any())
            {
                foreach (var provaAtualizada in provasAtualizadas)
                {
                    await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.QuestaoCompletaProva, provaAtualizada));
                }
            }

            return true;
        }
    }
}
