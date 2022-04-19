using MediatR;
using Sentry;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarAbrangenciaUsuarioGrupoExcluirUseCase : AbstractUseCase, ITratarAbrangenciaUsuarioGrupoExcluirUseCase
    {
        public TratarAbrangenciaUsuarioGrupoExcluirUseCase(IMediator mediator) : base(mediator) { }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var abrangencia = mensagemRabbit.ObterObjetoMensagem<Abrangencia>();
            if (abrangencia == null)
                throw new NegocioException("Abrangencia deve ser informada");

            var grupo = await mediator.Send(new ObterGrupoSerapPorIdQuery(abrangencia.GrupoId));
            var usuario = await mediator.Send(new ObterUsuarioSerapPorIdQuery(abrangencia.UsuarioId));

            var codigosAbrangencia = await mediator.Send(new ObterUeDreAtribuidasCoreSsoPorUsuarioEGrupoQuery(usuario.IdCoreSso, grupo.IdCoreSso));
            if (codigosAbrangencia == null || !codigosAbrangencia.Any())
                codigosAbrangencia = await mediator.Send(new ObterUeDreAtribuidasEolPorUsuarioQuery(usuario.Login));

            var dre = new Dre();
            if (abrangencia.DreId != null)
                dre = await mediator.Send(new ObterDrePorIdQuery((long)abrangencia.DreId));

            var ue = new Ue();
            if (abrangencia.UeId != null)
                ue = await mediator.Send(new ObterUePorIdQuery((long)abrangencia.UeId));

            var turma = new Turma();
            if (abrangencia.TurmaId != null)
                turma = await mediator.Send(new ObterTurmaSerapPorIdQuery((long)abrangencia.TurmaId));

            if (abrangencia.DreId != null && abrangencia.UeId == null && abrangencia.TurmaId is null)
                return await VerificarExcluirAbrangencia(codigosAbrangencia.ToArray(), dre.CodigoDre, abrangencia.Id);

            if (abrangencia.DreId != null && abrangencia.UeId != null && abrangencia.TurmaId is null)
                return await VerificarExcluirAbrangencia(codigosAbrangencia.ToArray(), ue.CodigoUe, abrangencia.Id);

            return true;
        }

        private async Task<bool> VerificarExcluirAbrangencia(string[] codigos, string codigoParaVerificar, long idAbrangencia)
        {
            if (!codigos.Any(c => c == codigoParaVerificar))
                await mediator.Send(new ExcluirAbrangenciaPorIdCommand(idAbrangencia));
            return true;
        }
    }
}
