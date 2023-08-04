using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExecutarSincronizacaoInstitucionalDreTratarUseCase : AbstractUseCase, IExecutarSincronizacaoInstitucionalDreTratarUseCase
    {
        public ExecutarSincronizacaoInstitucionalDreTratarUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var dre = param.ObterObjetoMensagem<DreParaSincronizacaoInstitucionalDto>();
            
            if (dre == null)
                throw new NegocioException("Não foi possível localizar a Dre para sincronizar as Ues.");
            
            await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.SincronizaEstruturaInstitucionalUesSync, dre));

            return true;
        }
    }
}