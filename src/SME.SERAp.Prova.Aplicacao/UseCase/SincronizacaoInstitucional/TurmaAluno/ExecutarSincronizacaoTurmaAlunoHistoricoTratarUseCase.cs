using MediatR;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using System.Linq;
using System.Threading.Tasks;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExecutarSincronizacaoTurmaAlunoHistoricoTratarUseCase : AbstractUseCase, IExecutarSincronizacaoTurmaAlunoHistoricoTratarUseCase
    {
        public ExecutarSincronizacaoTurmaAlunoHistoricoTratarUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var aluno = mensagemRabbit.ObterObjetoMensagem<AlunoParaSincronizacaoInstitucionalDto>();
            
            if (aluno == null)
                throw new NegocioException("Não foi possível localizar aluno para sincronizar o histórico.");

            var turmasHistoricoEol = (await mediator.Send(new ObterTurmaAlunoHistoricoEolPorAlunosRaQuery(new[] { aluno.AlunoCodigo }))).ToList();

            if (!turmasHistoricoEol.Any())
                return true;

            var codigosTrumas = turmasHistoricoEol.Select(c => c.CodigoTurma.ToString()).Distinct().ToArray();
            var turmas = (await mediator.Send(new ObterTurmaPorCodigosQuery(codigosTrumas))).ToList();

            var turmasHistoricoSerap = (await mediator.Send(new ObterTurmaAlunoHistoricoSerapPorAlunosRaQuery(new[] { aluno.AlunoCodigo }))).ToList();

            foreach (var turmaHistoricoEol in turmasHistoricoEol)
            {
                var turma = turmas.FirstOrDefault(c => c.Codigo == turmaHistoricoEol.CodigoTurma.ToString());
                
                if (turma == null)
                    continue;

                var turmaHistoricoSerap = turmasHistoricoSerap.FirstOrDefault(c =>
                    c.Matricula == turmaHistoricoEol.Matricula &&
                    c.AlunoRa == aluno.AlunoCodigo &&
                    c.TurmaId == turma.Id &&
                    c.AnoLetivo == turmaHistoricoEol.AnoLetivo);

                if (turmaHistoricoSerap == null)
                {
                    await mediator.Send(new TurmaAlunoHistoricoIncluirCommand(new TurmaAlunoHistorico(
                        turmaHistoricoEol.Matricula,
                        turma.Id,
                        turmaHistoricoEol.AnoLetivo, 
                        aluno.AlunoCodigo, 
                        turmaHistoricoEol.DataMatricula, 
                        turmaHistoricoEol.DataSituacao))
                    );                    
                }
                else
                {
                    await mediator.Send(new TurmaAlunoHistoricoAlterarCommand(new TurmaAlunoHistorico(
                        turmaHistoricoSerap.Id, 
                        turmaHistoricoEol.Matricula,
                        turma.Id, 
                        turmaHistoricoEol.AnoLetivo, 
                        aluno.AlunoCodigo, 
                        turmaHistoricoEol.DataMatricula, 
                        turmaHistoricoEol.DataSituacao))
                    );                    
                }
            }

            return true;
        }
    }
}
