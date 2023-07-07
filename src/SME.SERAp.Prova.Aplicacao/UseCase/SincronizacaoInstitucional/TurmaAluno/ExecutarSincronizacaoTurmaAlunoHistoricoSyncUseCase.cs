using System;
using MediatR;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.SERAp.Prova.Infra.Interfaces;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExecutarSincronizacaoTurmaAlunoHistoricoSyncUseCase : AbstractUseCase, IExecutarSincronizacaoTurmaAlunoHistoricoSyncUseCase
    {
        private readonly IServicoLog servicoLog;
        
        public ExecutarSincronizacaoTurmaAlunoHistoricoSyncUseCase(IMediator mediator, IServicoLog servicoLog) : base(mediator)
        {
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var turmas = mensagemRabbit.ObterObjetoMensagem<List<TurmaParaSincronizacaoInstitucionalDto>>();
                
                if (turmas == null || !turmas.Any())
                    throw new NegocioException("Não foi possível localizar as turmas para sincronizar históricos dos alunos.");

                var turmasIds = turmas.Select(c => c.Id).ToArray();
                var todosAlunosTurmaSerap = await mediator.Send(new ObterAlunosSerapPorTurmasIdsQuery(turmasIds));

                var alunosParaSincronizacaoInstitucional = todosAlunosTurmaSerap.Select(alunoTurma =>
                        new AlunoParaSincronizacaoInstitucionalDto(alunoTurma.Id, alunoTurma.RA, alunoTurma.TurmaId))
                    .ToList();

                for (var i = 0; i < alunosParaSincronizacaoInstitucional.Count; i+= 10)
                {
                    var alunosParaTratar = alunosParaSincronizacaoInstitucional.Skip(i).Take(10);
                    
                    await mediator.Send(new PublicaFilaRabbitCommand(
                        RotasRabbit.SincronizaEstruturaInstitucionalTurmaAlunoHistoricoTratar,
                        alunosParaTratar));
                }

                return true;
            }
            catch (Exception e)
            {
                servicoLog.Registrar($"Erro ao sincronizar as Turmas Alunos Históricos: {mensagemRabbit.Mensagem}", e);
                throw;
            }
        }
    }
}
