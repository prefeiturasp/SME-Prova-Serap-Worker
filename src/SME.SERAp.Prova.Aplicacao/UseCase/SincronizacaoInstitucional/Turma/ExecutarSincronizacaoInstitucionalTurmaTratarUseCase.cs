﻿using System.Collections.Generic;
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
            var turmas = param.ObterObjetoMensagem<List<TurmaParaSincronizacaoInstitucionalDto>>();
            
            if (turmas == null)
                throw new NegocioException("Não foi possível localizar as turmas para sincronizar os alunos e atualizar a ue das turmas.");
            
            await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.SincronizaEstruturaInstitucionalAlunoSync, turmas,
                param.CodigoCorrelacao));
            
            await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.SincronizaEstruturaInstitucionalTurmaAlunoHistoricoSync, turmas,
                param.CodigoCorrelacao));
            
            await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.SincronizaEstruturaInstitucionalAtualizarUeTurma, turmas,
                param.CodigoCorrelacao));

            return true;
        }
    }
}