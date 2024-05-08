using System.Collections.Generic;
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
            var alunos = mensagemRabbit.ObterObjetoMensagem<List<AlunoParaSincronizacaoInstitucionalDto>>();
            
            if (alunos == null || !alunos.Any())
                throw new NegocioException("Não foi possível localizar os alunos para sincronizar os históricos.");

            var alunosCodigos = alunos.Select(c => c.AlunoCodigo).ToArray();
            var turmasHistoricoEol = (await mediator.Send(new ObterTurmaAlunoHistoricoEolPorAlunosRaQuery(alunosCodigos))).ToList();

            if (!turmasHistoricoEol.Any())
                return true;

            var codigosTrumas = turmasHistoricoEol.Select(c => c.CodigoTurma.ToString()).Distinct().ToArray();
            var turmas = (await mediator.Send(new ObterTurmaPorCodigosQuery(codigosTrumas))).ToList();

            var turmasHistoricoSerap = (await mediator.Send(new ObterTurmaAlunoHistoricoSerapPorAlunosRaQuery(alunosCodigos))).ToList();

            foreach (var turmaHistoricoEol in turmasHistoricoEol)
            {
                var turma = turmas.FirstOrDefault(c => c.Codigo == turmaHistoricoEol.CodigoTurma.ToString());
                
                if (turma == null)
                    continue;

                var aluno = alunos.FirstOrDefault(c => c.AlunoCodigo == turmaHistoricoEol.CodigoAluno);
                
                if (aluno == null)
                    continue;

                var turmaHistoricoSerap = turmasHistoricoSerap.FirstOrDefault(c =>
                    c.Matricula == turmaHistoricoEol.CodigoMatricula &&
                    c.AlunoRa == turmaHistoricoEol.CodigoAluno &&
                    c.TurmaId == turma.Id &&
                    c.AnoLetivo == turmaHistoricoEol.AnoLetivo);

                if (turmaHistoricoSerap == null)
                {
                    await mediator.Send(new TurmaAlunoHistoricoIncluirCommand(new TurmaAlunoHistorico(
                        turmaHistoricoEol.CodigoMatricula,
                        turma.Id,
                        turmaHistoricoEol.AnoLetivo, 
                        aluno.Id, 
                        turmaHistoricoEol.DataMatricula, 
                        turmaHistoricoEol.DataSituacao))
                    );                    
                }
                else
                {
                    await mediator.Send(new TurmaAlunoHistoricoAlterarCommand(new TurmaAlunoHistorico(
                        turmaHistoricoSerap.Id, 
                        turmaHistoricoEol.CodigoMatricula,
                        turma.Id, 
                        turmaHistoricoEol.AnoLetivo, 
                        aluno.Id, 
                        turmaHistoricoEol.DataMatricula, 
                        turmaHistoricoEol.DataSituacao))
                    );                    
                }
            }

            return true;
        }
    }
}
