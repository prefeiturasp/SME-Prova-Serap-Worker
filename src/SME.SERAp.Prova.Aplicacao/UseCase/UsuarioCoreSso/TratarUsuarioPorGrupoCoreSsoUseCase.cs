using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarUsuarioPorGrupoCoreSsoUseCase : AbstractUseCase, ITratarUsuarioPorGrupoCoreSsoUseCase
    {

        public TratarUsuarioPorGrupoCoreSsoUseCase(IMediator mediator) : base(mediator){}

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var grupo = mensagemRabbit.ObterObjetoMensagem<GrupoSerapCoreSso>();
            var usuariosGrupo = await mediator.Send(new ObterUsuariosPorGrupoCoreSsoQuery(grupo.IdCoreSso));

            foreach (var usuario in usuariosGrupo)
            {
                var usuarioMsg = new UsuarioGrupoDto(grupo.Id, usuario);
                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.UsuarioCoreSsoTratar, usuarioMsg));
            }

            return true;
        }
    }
}
