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
            var usuarioGrupoSerap = mensagemRabbit.ObterObjetoMensagem<UsuarioGrupoSerapDto>();
            if (usuarioGrupoSerap == null)
                throw new NegocioException("Sync abrangência - Usuário e Grupo não informado.");

            var usuarioSerap = await mediator.Send(new ObterUsuarioSerapPorIdQuery(usuarioGrupoSerap.IdUsuarioSerap));
            if (usuarioSerap == null)
                throw new NegocioException($"Sync abrangência - Usuário não encontrado, IdUsuario:{usuarioGrupoSerap.IdUsuarioSerap}.");

            var grupoSerap = await mediator.Send(new ObterGrupoSerapPorIdQuery(usuarioGrupoSerap.IdGrupoSerap));
            if (grupoSerap == null)
                throw new NegocioException($"Sync abrangência - Grupo não encontrado, IdGrupo:{usuarioGrupoSerap.IdGrupoSerap}.");

            if (grupoSerap.IdCoreSso == GruposCoreSso.Professor || grupoSerap.IdCoreSso == GruposCoreSso.Professor_old)
                throw new NegocioException("Abrangência de professor ainda não está sendo tratada.");

            string parametrosMsgLog = ObterParametrosMsgLog(usuarioSerap, grupoSerap);

            var abrangencia = await mediator.Send(new ObterUeDreAtribuidasCoreSsoPorUsuarioEGrupoQuery(usuarioSerap.IdCoreSso, grupoSerap.IdCoreSso));
            if (abrangencia == null || !abrangencia.Any())
                abrangencia = await mediator.Send(new ObterUeDreAtribuidasEolPorUsuarioQuery(usuarioSerap.Login));

            if (abrangencia == null || !abrangencia.Any())
            {
                SentrySdk.CaptureMessage($"Abrangência do usuário não encontrada. {parametrosMsgLog}", SentryLevel.Warning);
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
                        SentrySdk.CaptureMessage($"Sync abrangência - Ue não encontrada: {codigo}. {parametrosMsgLog}", SentryLevel.Warning);
                }
            }

            foreach (Abrangencia abrangenciaInserir in novaAbrangencia)
            {
                await mediator.Send(new InserirAbrangenciaCommand(abrangenciaInserir));
            }

            return true;
        }

        private string ObterParametrosMsgLog(UsuarioSerapCoreSso usuario, GrupoSerapCoreSso grupo)
        {
            return $"UsuarioId: {usuario.Id}, UsuarioIdCoreSSO: {usuario.IdCoreSso}, UsuarioLogin: {usuario.Login}, GrupoId: {grupo.Id}, GrupoIdCoreSSO: {grupo.IdCoreSso}";
        }
    }
}
