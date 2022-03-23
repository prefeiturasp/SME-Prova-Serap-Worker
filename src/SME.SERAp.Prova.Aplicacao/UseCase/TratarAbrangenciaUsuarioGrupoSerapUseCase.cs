using MediatR;
using Sentry;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
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

                var abrangencia = await mediator.Send(new ObterUeDreAtribuidasCoreSsoPorUsuarioEGrupoQuery(usuarioSerap.IdCoreSso, grupoSerap.IdCoreSso));
                if (abrangencia == null || !abrangencia.Any())
                    abrangencia = await mediator.Send(new ObterUeDreAtribuidasEolPorUsuarioQuery(usuarioSerap.Login));

                if (abrangencia == null || !abrangencia.Any())
                {
                    SentrySdk.CaptureMessage($"Abrangência do usuário não encontrada. UsuarioId: {usuarioSerap.Id}, UsuarioIdCoreSSO: {usuarioSerap.IdCoreSso}, UsuarioLogin: {usuarioSerap.Login}", SentryLevel.Warning);
                    return true;
                }

                var novaAbrangencia = new List<Abrangencia>();
                var dresSerap = await mediator.Send(new ObterDresSerapQuery());
                foreach (string codigo in abrangencia)
                {
                    var abrangenciaItem = new Abrangencia(usuarioSerap.Id, grupoSerap.Id);
                    var dre = dresSerap.Where(d => d.CodigoDre == codigo).FirstOrDefault();
                    if (dre != null)
                    {
                        abrangenciaItem.DreId = dre.Id;
                        novaAbrangencia.Add(abrangenciaItem);
                    }
                    else
                    {
                        var ue = await mediator.Send(new ObterUePorCodigoQuery(codigo));
                        if (ue != null)
                        {
                            abrangenciaItem.DreId = ue.DreId;
                            abrangenciaItem.UeId = ue.Id;
                            novaAbrangencia.Add(abrangenciaItem);
                        }
                        else
                            SentrySdk.CaptureMessage($"Sync abrangência - Ue não encontrada: {codigo}. UsuarioId: {usuarioSerap.Id}, UsuarioIdCoreSSO: {usuarioSerap.IdCoreSso}, UsuarioLogin: {usuarioSerap.Login}", SentryLevel.Warning);
                    }
                }

                //InserirAbrangenciaCommand

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
