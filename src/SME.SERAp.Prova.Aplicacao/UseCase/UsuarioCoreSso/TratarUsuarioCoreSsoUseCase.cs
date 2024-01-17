using MediatR;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarUsuarioCoreSsoUseCase : AbstractUseCase, ITratarUsuarioCoreSsoUseCase
    {
        public TratarUsuarioCoreSsoUseCase(IMediator mediator) : base(mediator) { }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var usuarioGrupo = mensagemRabbit.ObterObjetoMensagem<UsuarioGrupoDto>();
            if (usuarioGrupo == null)
                throw new NegocioException("Usuário e Grupo não informado.");

            var usuarioGrupoSerap = await TratarUsuario(usuarioGrupo);
            await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.UsuarioGrupoAbrangenciaTratar, new UsuarioGrupoSerapDto(usuarioGrupoSerap.IdUsuarioSerapCoreSso, usuarioGrupoSerap.IdGrupoSerapCoreSso)));

            return true;
        }

        private async Task<UsuarioGrupoSerapCoreSso> TratarUsuario(UsuarioGrupoDto usuarioGrupo)
        {
            var usuarioCoreSso = usuarioGrupo.UsuarioCoreSso;
            var usuarioSerap = await mediator.Send(new ObterUsuarioSerapCoreSsoPorIdCoreSsoQuery(usuarioCoreSso.IdCoreSso));

            var usuarioGrupoSerap = new UsuarioGrupoSerapCoreSso(usuarioGrupo.IdGrupo);

            if (usuarioSerap == null)
            {
                var usuarioInserir = new UsuarioSerapCoreSso(usuarioCoreSso.IdCoreSso, usuarioCoreSso.Login, usuarioCoreSso.Nome);
                usuarioInserir.Id = await mediator.Send(new InserirUsuarioSerapCoreSsoCommand(usuarioInserir));
                usuarioGrupoSerap.IdUsuarioSerapCoreSso = usuarioInserir.Id;
                await mediator.Send(new InserirUsuarioGrupoSerapCoreSsoCommand(usuarioGrupoSerap));
            }
            else
            {
                if (usuarioSerap.Login != usuarioCoreSso.Login || usuarioSerap.Nome != usuarioCoreSso.Nome)
                {
                    usuarioSerap.Login = usuarioCoreSso.Login;
                    usuarioSerap.Nome = usuarioCoreSso.Nome;
                    usuarioSerap.AtualizarDataAtualizadoEm();
                    await mediator.Send(new AlterarUsuarioSerapCoreSsoCommand(usuarioSerap));
                }

                usuarioGrupoSerap.IdUsuarioSerapCoreSso = usuarioSerap.Id;
                var usuarioGrupoVerificar = await mediator.Send(new ObterUsuarioGrupoSerapPorUsuarioIdEGrupoIdQuery(usuarioSerap.Id, usuarioGrupo.IdGrupo));
                if (usuarioGrupoVerificar == null)
                {                    
                    await mediator.Send(new InserirUsuarioGrupoSerapCoreSsoCommand(usuarioGrupoSerap));
                }
            }

            return usuarioGrupoSerap;
        }
    }
}
