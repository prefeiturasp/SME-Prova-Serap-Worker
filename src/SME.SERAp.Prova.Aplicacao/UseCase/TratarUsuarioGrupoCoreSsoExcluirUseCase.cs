using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarUsuarioGrupoCoreSsoExcluirUseCase : AbstractUseCase, ITratarUsuarioGrupoCoreSsoExcluirUseCase
    {
        public TratarUsuarioGrupoCoreSsoExcluirUseCase(IMediator mediator) : base(mediator) { }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var grupo = mensagemRabbit.ObterObjetoMensagem<GrupoSerapCoreSso>();

            var usuariosGrupoCoreSso = await mediator.Send(new ObterUsuariosPorGrupoCoreSsoQuery(grupo.IdCoreSso));
            var usuariosGrupoSerap = await mediator.Send(new ObterUsuariosSerapPorGrupoSerapIdQuery(grupo.Id));

            var usuarioParaExcluir = usuariosGrupoSerap.Where(u => !usuariosGrupoCoreSso.Any(ugc => ugc.IdCoreSso == u.IdCoreSso));

            foreach (UsuarioSerapCoreSso usuario in usuarioParaExcluir)
            {
                await mediator.Send(new ExcluirUsuarioGrupoSerapPorUsuarioIdEGrupoIdCommand(usuario.Id, grupo.Id));
            }

            return true;
        }
    }
}
