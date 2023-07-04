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
                var turma = mensagemRabbit.ObterObjetoMensagem<TurmaParaSincronizacaoInstitucionalDto>();
            
                if (turma == null)
                    throw new NegocioException("Não foi possível localizar a turma para sincronizar históricos dos alunos.");

                var todosAlunosTurmaSerap = await mediator.Send(new ObterAlunosSerapPorTurmasCodigoQuery(new [] { turma.Id }));

                foreach (var alunoTurma in todosAlunosTurmaSerap)
                {
                    await mediator.Send(new PublicaFilaRabbitCommand(
                        RotasRabbit.SincronizaEstruturaInstitucionalTurmaAlunoHistoricoTratar,
                        new AlunoParaSincronizacaoInstitucionalDto(alunoTurma.RA, alunoTurma.TurmaId),
                        mensagemRabbit.CodigoCorrelacao));
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
