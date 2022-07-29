using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class VersaoAppDispositivoAppUseCase : AbstractUseCase, IVersaoAppDispositivoAppUseCase
    {
        public VersaoAppDispositivoAppUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var versaoApp = mensagemRabbit.ObterObjetoMensagem<VersaoAppDispositivo>();
            return await mediator.Send(new IncluirVersaoAppDispositivoCommand(versaoApp));
        }
    }
}
