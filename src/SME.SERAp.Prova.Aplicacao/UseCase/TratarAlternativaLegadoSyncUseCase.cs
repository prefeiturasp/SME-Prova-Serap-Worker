using MediatR;
using SME.SERAp.Prova.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarAlternativaLegadoSyncUseCase : ITratarAlternativaLegadoSyncUseCase
    {
        private readonly IMediator mediator;

        public TratarAlternativaLegadoSyncUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var busca = mensagemRabbit.ObterObjetoMensagem<BuscarPorProvaIdEQuestaoIdDto>();


            var alternativasId =
                await mediator.Send(new ObterAlternativasLegadoPorIdQuery(busca.QuestaoId));

            foreach (var id in alternativasId)
            {
                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.AlternativaTratar,
                    new DetalheAlternativaDto(busca.ProvaId, busca.QuestaoId, id)));
            }
            return true;
        }
    }
}