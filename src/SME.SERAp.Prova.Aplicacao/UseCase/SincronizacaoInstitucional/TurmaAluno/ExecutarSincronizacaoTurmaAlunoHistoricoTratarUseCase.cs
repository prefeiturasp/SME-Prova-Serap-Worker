using MediatR;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExecutarSincronizacaoTurmaAlunoHistoricoTratarUseCase : AbstractUseCase, IExecutarSincronizacaoTurmaAlunoHistoricoTratarUseCase
    {
        public ExecutarSincronizacaoTurmaAlunoHistoricoTratarUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var alunosRa = mensagemRabbit.ObterObjetoMensagem<long[]>();

            if(alunosRa == null)
            {
                var mensagem = $"Não foi possível fazer parse da mensagem para tratar turmas histórico dos alunos. {mensagemRabbit.Mensagem}.";
                throw new NegocioException(mensagem);
            }

            var turmasHistoricoEol = await mediator.Send(new ObterTurmaAlunoHistoricoEolPorAlunosRaQuery(alunosRa));
            
            var turmasCodigo = turmasHistoricoEol.Select(t => t.CodigoTurma.ToString()).Distinct().ToArray();
            var turmas = await mediator.Send(new ObterTurmaPorCodigosQuery(turmasCodigo));

            var turmasHistoricoSerap = await mediator.Send(new ObterTurmaAlunoHistoricoSerapPorAlunosRaQuery(alunosRa));

            foreach (var turmaHistoricoEol in turmasHistoricoEol)
            {
                var turma = turmas.FirstOrDefault(t => t.Codigo == turmaHistoricoEol.CodigoTurma.ToString());
                var aluno = turmasHistoricoSerap.FirstOrDefault(t => t.AlunoRa == turmaHistoricoEol.AlunoRa);

                if (turma != null && aluno != null)
                {
                    if (!turmasHistoricoSerap.Any(t =>
                        t.AlunoRa == turmaHistoricoEol.AlunoRa &&
                        t.TurmaId == turma.Id &&
                        t.AnoLetivo == turmaHistoricoEol.AnoLetivo &&
                        t.DataMatricula == turmaHistoricoEol.DataMatricula &&
                        t.DataSituacao == turmaHistoricoEol.DataSituacao
                    ))
                    {
                        await mediator.Send(new TurmaAlunoHistoricoIncluirCommand(new Dominio.TurmaAlunoHistorico(turma.Id, turmaHistoricoEol.AnoLetivo, aluno.AlunoId, turmaHistoricoEol.DataMatricula, turmaHistoricoEol.DataSituacao)));
                    }
                }
            }

            return true;
        }
    }
}
