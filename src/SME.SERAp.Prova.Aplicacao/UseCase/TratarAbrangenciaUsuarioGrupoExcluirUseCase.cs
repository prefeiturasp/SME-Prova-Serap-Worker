using MediatR;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
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
            if (grupo == null)
                throw new NegocioException("Grupo não localizado.");
            
            var usuario = await mediator.Send(new ObterUsuarioSerapPorIdQuery(abrangencia.UsuarioId));
            if (usuario == null)
                throw new NegocioException("Usuário não localizado.");

            var dre = new Dre();
            if (abrangencia.DreId != null)
            {
                var dreId = abrangencia.DreId.GetValueOrDefault();
                dre = await mediator.Send(new ObterDrePorIdQuery(dreId));
                
                if (dre == null)
                    throw new NegocioException($"Dre {dreId} não localizada.");
            }

            var ue = new Ue();
            if (abrangencia.UeId != null)
            {
                var ueId = abrangencia.UeId.GetValueOrDefault();
                ue = await mediator.Send(new ObterUePorIdQuery(ueId));
                
                if (ue == null)
                    throw new NegocioException($"Ue {ueId} não localizada.");                
            }

            var turma = new Turma();
            if (abrangencia.TurmaId != null)
            {
                var turmaId = abrangencia.TurmaId.GetValueOrDefault();
                turma = await mediator.Send(new ObterTurmaSerapPorIdQuery(turmaId));
                
                if (turma == null)
                    throw new NegocioException($"Turma {turmaId} não localizada.");                
            }

            if (grupo.IdCoreSso == GruposCoreSso.Professor || grupo.IdCoreSso == GruposCoreSso.Professor_old)
            {
                var atribuicoes = await mediator.Send(new ObterTurmaAtribuidasEolPorUsuarioQuery(usuario.Login, long.Parse(turma.Codigo), turma.AnoLetivo));

                if (atribuicoes != null && !atribuicoes.Any(t => t.AnoLetivo == turma.AnoLetivo && t.TurmaCodigo.ToString() == turma.Codigo))
                    await mediator.Send(new ExcluirAbrangenciaPorIdCommand(abrangencia.Id));
            }
            else
            {
                var codigosAbrangencia = await mediator.Send(new ObterUeDreAtribuidasCoreSsoPorUsuarioEGrupoQuery(usuario.IdCoreSso, grupo.IdCoreSso));
                if (codigosAbrangencia == null || !codigosAbrangencia.Any())
                    codigosAbrangencia = await mediator.Send(new ObterUeDreAtribuidasEolPorUsuarioQuery(usuario.Login));

                if (codigosAbrangencia != null)
                {
                    if (abrangencia.DreId != null && abrangencia.UeId == null && abrangencia.TurmaId is null)
                        await VerificarExcluirAbrangencia(codigosAbrangencia.ToArray(), dre.CodigoDre, abrangencia.Id);

                    if (abrangencia.DreId != null && abrangencia.UeId != null && abrangencia.TurmaId is null)
                        await VerificarExcluirAbrangencia(codigosAbrangencia.ToArray(), ue.CodigoUe, abrangencia.Id);
                }
            }

            return true;
        }

        private async Task VerificarExcluirAbrangencia(string[] codigos, string codigoParaVerificar, long idAbrangencia)
        {
            if (!codigos.Any(c => c == codigoParaVerificar))
                await mediator.Send(new ExcluirAbrangenciaPorIdCommand(idAbrangencia));
        }
    }
}
