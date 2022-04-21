using MediatR;
using Sentry;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
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

            string parametrosMsgLog = ObterParametrosMsgLog(usuarioSerap, grupoSerap);
            if (grupoSerap.IdCoreSso == GruposCoreSso.Professor || grupoSerap.IdCoreSso == GruposCoreSso.Professor_old)
            {
                var atribuicoes = await mediator.Send(new ObterTurmaAtribuidasEolPorUsuarioQuery(usuarioSerap.Login));

                TurmaAtribuicaoDto turma;
                Abrangencia abrangencia;
                foreach (var atribuicao in atribuicoes)
                {
                    turma = await mediator.Send(new ObterTurmaPorCodigoQuery(atribuicao.AnoLetivo, atribuicao.TurmaCodigo.ToString()));
                    if (turma == null)
                        continue;

                    abrangencia = await mediator.Send(new ObterAbrangenciaPorUsuarioGrupoDreUeTurmaQuery(usuarioSerap.Id,
                        grupoSerap.Id,
                        turma.DreId,
                         turma.UeId,
                        turma.TurmaId));

                    if (abrangencia == null)
                    {
                        abrangencia = new Abrangencia(
                            usuarioSerap.Id,
                            grupoSerap.Id,
                            turma.DreId,
                             turma.UeId,
                            turma.TurmaId,
                            atribuicao.DataAtribuicao,
                            atribuicao.DataDisponibilizacaoAula);

                        await mediator.Send(new InserirAbrangenciaCommand(abrangencia));
                    }
                    else if(abrangencia.Inicio != atribuicao.DataAtribuicao || abrangencia.Fim != atribuicao.DataDisponibilizacaoAula)
                    {
                        abrangencia.Inicio = atribuicao.DataAtribuicao;
                        abrangencia.Fim = atribuicao.DataDisponibilizacaoAula;

                        await mediator.Send(new AlterarAbrangenciaCommand(abrangencia));
                    }
                }
            }
            else
            {
                var abrangencias = await mediator.Send(new ObterUeDreAtribuidasCoreSsoPorUsuarioEGrupoQuery(usuarioSerap.IdCoreSso, grupoSerap.IdCoreSso));
                if (abrangencias == null || !abrangencias.Any())
                    abrangencias = await mediator.Send(new ObterUeDreAtribuidasEolPorUsuarioQuery(usuarioSerap.Login));

                if (abrangencias == null || !abrangencias.Any())
                {
                    SentrySdk.CaptureMessage($"Abrangência do usuário não encontrada. {parametrosMsgLog}", SentryLevel.Warning);
                    return true;
                }

                Abrangencia abrangencia;
                var dresSerap = await mediator.Send(new ObterDresSerapQuery());
                foreach (string codigo in abrangencias)
                {
                    var dre = dresSerap.Where(d => d.CodigoDre == codigo).FirstOrDefault();
                    if (dre != null)
                    {
                        abrangencia = abrangencia = new Abrangencia(
                            usuarioSerap.Id,
                            grupoSerap.Id,
                            dre.Id);

                        await mediator.Send(new InserirAbrangenciaCommand(abrangencia));
                    }
                    else
                    {
                        var ue = await mediator.Send(new ObterUePorCodigoQuery(codigo));
                        if (ue != null)
                        {
                            abrangencia = abrangencia = new Abrangencia(
                                usuarioSerap.Id,
                                grupoSerap.Id,
                                ue.DreId,
                                ue.Id);

                            await mediator.Send(new InserirAbrangenciaCommand(abrangencia));
                        }
                        else
                            SentrySdk.CaptureMessage($"Sync abrangência - Ue não encontrada: {codigo}. {parametrosMsgLog}", SentryLevel.Warning);
                    }
                }
            }

            return true;
        }

        private string ObterParametrosMsgLog(UsuarioSerapCoreSso usuario, GrupoSerapCoreSso grupo)
        {
            return $"UsuarioId: {usuario.Id}, UsuarioIdCoreSSO: {usuario.IdCoreSso}, UsuarioLogin: {usuario.Login}, GrupoId: {grupo.Id}, GrupoIdCoreSSO: {grupo.IdCoreSso}";
        }
    }
}
