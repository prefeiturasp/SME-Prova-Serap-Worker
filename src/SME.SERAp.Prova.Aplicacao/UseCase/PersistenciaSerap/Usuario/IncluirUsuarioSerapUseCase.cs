using MediatR;
using SME.SERAp.Prova.Aplicacao.Commands;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class IncluirUsuarioSerapUseCase : AbstractUseCase, IIncluirUsuarioSerapUseCase
    {
        public IncluirUsuarioSerapUseCase(IMediator mediator) : base(mediator)
        {
        }
        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var usuario = mensagemRabbit.ObterObjetoMensagem<UsuarioDto>();
            return await mediator.Send(new IncluirUsuarioCommand(usuario.Login, usuario.Nome));

        }
    }
}
