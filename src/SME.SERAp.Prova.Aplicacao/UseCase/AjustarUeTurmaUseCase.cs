using MediatR;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AjustarUeTurmaUseCase : AbstractUseCase, IAjustarUeTurmaUseCase
    {
        public AjustarUeTurmaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var turma = mensagemRabbit.ObterObjetoMensagem<TurmaParaSincronizacaoInstitucionalDto>();
            
            if (turma == null)
                throw new NegocioException("Não foi possível localizar a turma para atualizar a ue.");

            var ueCorretaSerap = await mediator.Send(new ObterUePorCodigoQuery(turma.UeCodigo));
            
            if (ueCorretaSerap == null)
                return true;
            
            var turmaParaAtualizarUe = await mediator.Send(new ObterTurmaSerapPorIdQuery(turma.Id));

            if (turmaParaAtualizarUe == null)
                return true;
            
            turmaParaAtualizarUe.UeId = ueCorretaSerap.Id;
            turmaParaAtualizarUe.DataAtualizacao = DateTime.Now;
            await mediator.Send(new AtualizarTurmaCommand(turmaParaAtualizarUe));

            return true;
        }
    }
}
