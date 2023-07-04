using MediatR;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExecutarSincronizacaoInstitucionalUeTratarUseCase : AbstractUseCase, IExecutarSincronizacaoInstitucionalUeTratarUseCase
    {
        public ExecutarSincronizacaoInstitucionalUeTratarUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var ue = mensagemRabbit.ObterObjetoMensagem<UeParaSincronizacaoInstitucionalDto>();

            if (ue == null)
                throw new NegocioException("Não foi possível localizar a Ue para sincronizar as turmas.");
            
            await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.SincronizaEstruturaInstitucionalTurmasSync, ue,
                mensagemRabbit.CodigoCorrelacao));

            return true;
        }
    }
}
