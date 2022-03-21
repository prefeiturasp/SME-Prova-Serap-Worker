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
    public class TratarAbrangenciaUsuarioGrupoSerapUseCase : AbstractUseCase, ITratarAbrangenciaUsuarioGrupoSerapUseCase
    {
        public TratarAbrangenciaUsuarioGrupoSerapUseCase(IMediator mediator) : base(mediator) { }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var usuarioGrupoSerap = mensagemRabbit.ObterObjetoMensagem<UsuarioGrupoSerapCoreSso>();
                if (usuarioGrupoSerap == null)
                    throw new NegocioException("Usuário e Grupo não informado.");

                var usuarioSerap = await mediator.Send(new ObterUsuarioSerapPorIdQuery(usuarioGrupoSerap.IdUsuarioSerapCoreSso));
                var grupoSerap = await mediator.Send(new ObterGrupoSerapPorIdQuery(usuarioGrupoSerap.IdGrupoSerapCoreSso));

                return true;
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureMessage($"Tratar abrangência usuário. msg: {mensagemRabbit.Mensagem}", SentryLevel.Error);
                SentrySdk.CaptureException(ex);
                return false;
            }
        }
    }
}
