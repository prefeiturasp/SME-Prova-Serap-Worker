using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExecutarSincronizacaoInstitucionalTurmaTratarUseCase : AbstractUseCase, IExecutarSincronizacaoInstitucionalTurmaTratarUseCase
    {
        public ExecutarSincronizacaoInstitucionalTurmaTratarUseCase(IMediator mediator) : base(mediator)
        {
        }        
        
        public async Task<bool> Executar(MensagemRabbit param)
        {
            var turma = param.ObterObjetoMensagem<TurmaParaSincronizacaoInstitucionalDto>();

            if (turma == null)
                throw new NegocioException("Não foi possível localizar a turma para sincronizar os alunos e atualizar a ue da turma.");
            
            await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.SincronizaEstruturaInstitucionalAlunoSync, turma,
                param.CodigoCorrelacao));

            await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.SincronizaEstruturaInstitucionalTurmaAlunoHistoricoSync, turma,
                param.CodigoCorrelacao));
            
            await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.SincronizaEstruturaInstitucionalAtualizarUeTurma, turma,
                param.CodigoCorrelacao));            

            return true;
        }
    }
}