﻿using MediatR;
using Sentry;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarUsuarioPorGrupoCoreSsoUseCase : AbstractUseCase, ITratarUsuarioPorGrupoCoreSsoUseCase
    {

        public TratarUsuarioPorGrupoCoreSsoUseCase(IMediator mediator) : base(mediator){}

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {            
            try
            {
                var grupo = mensagemRabbit.ObterObjetoMensagem<GrupoSerapCoreSso>();
                var usuariosGrupo = await mediator.Send(new ObterUsuariosPorGrupoCoreSsoQuery(grupo.IdCoreSso));
                foreach(UsuarioCoreSsoDto usuario in usuariosGrupo)
                {
                    var usuarioMsg = new UsuarioGrupoDto(grupo.Id, usuario);
                    await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.UsuarioCoreSsoTratar, usuarioMsg));
                }
                return true;
            }
            catch(Exception ex)
            {
                SentrySdk.CaptureMessage($"Tratar usuários do grupo. msg: {mensagemRabbit.Mensagem}", SentryLevel.Error);
                SentrySdk.CaptureException(ex);
                return false;
            }
        }
    }
}
