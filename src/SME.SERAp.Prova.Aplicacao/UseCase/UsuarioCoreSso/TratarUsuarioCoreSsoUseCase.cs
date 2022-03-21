using MediatR;
using Sentry;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarUsuarioCoreSsoUseCase : AbstractUseCase, ITratarUsuarioCoreSsoUseCase
    {

        public TratarUsuarioCoreSsoUseCase(IMediator mediator) : base(mediator) { }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var usuarioGrupo = mensagemRabbit.ObterObjetoMensagem<UsuarioGrupoDto>();
                if (usuarioGrupo == null)
                    throw new NegocioException("Usuário e Grupo não informado.");

                await TratarUsuario(usuarioGrupo);

                return true;
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureMessage($"Tratar usuário. msg: {mensagemRabbit.Mensagem}", SentryLevel.Error);
                SentrySdk.CaptureException(ex);
                return false;
            }
        }

        private async Task TratarUsuario(UsuarioGrupoDto usuarioGrupo)
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
                if (usuarioSerap.Login != usuarioCoreSso.Login
                    || usuarioSerap.Nome != usuarioCoreSso.Nome)
                {
                    usuarioSerap.Login = usuarioCoreSso.Login;
                    usuarioSerap.Nome = usuarioCoreSso.Nome;
                    usuarioSerap.AtualizarDataAtualizadoEm();
                    await mediator.Send(new AlterarUsuarioSerapCoreSsoCommand(usuarioSerap));
                }

                var usuarioGrupoVerificar = await mediator.Send(new ObterUsuarioGrupoSerapPorUsuarioIdEGrupoIdQuery(usuarioSerap.Id, usuarioGrupo.IdGrupo));
                if (usuarioGrupoVerificar == null)
                {
                    usuarioGrupoSerap.IdUsuarioSerapCoreSso = usuarioSerap.Id;
                    await mediator.Send(new InserirUsuarioGrupoSerapCoreSsoCommand(usuarioGrupoSerap));
                }
            }
        }
    }
}
