using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AlterarUsuarioSerapUseCase : AbstractUseCase, IAlterarUsuarioSerapUseCase
    {
        public AlterarUsuarioSerapUseCase(IMediator mediator) : base(mediator)
        {
        }
        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var usuario = mensagemRabbit.ObterObjetoMensagem<Usuario>();
            return await mediator.Send(new AlterarUsuarioCommand(usuario));
        }
    }
}
