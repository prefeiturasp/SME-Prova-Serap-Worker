using System.Threading.Tasks;
using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Interfaces;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarResultadoParticipacaoSmeCicloUseCase : AbstractTratarProficienciaPspUseCase, ITratarResultadoParticipacaoSmeCicloUseCase
    {
        public TratarResultadoParticipacaoSmeCicloUseCase(IMediator mediator, IServicoLog servicoLog, IModel model) :
            base(mediator, servicoLog, model)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            throw new System.NotImplementedException();
        }
    }
}