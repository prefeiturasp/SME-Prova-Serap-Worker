using MediatR;
using SME.SERAp.Prova.Infra;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ProvaWebPushTesteUseCase : IProvaWebPushTesteUseCase
    {
        private readonly IMediator mediator;

        public ProvaWebPushTesteUseCase(IMediator mediator) => this.mediator = mediator;

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var anoDto = mensagemRabbit.ObterObjetoMensagem<AnoAlunoDto>();

            await mediator.Send(new EnviarNotificacaoPorAnoCommand(anoDto.Ano, "Nova prova disponível"));

            return default;
        }
    }
}
