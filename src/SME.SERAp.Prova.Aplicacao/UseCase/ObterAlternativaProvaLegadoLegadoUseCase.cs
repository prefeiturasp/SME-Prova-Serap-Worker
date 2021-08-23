﻿using System;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlternativaProvaLegadoLegadoUseCase : IObterAlternativaProvaLegadoLegadoUseCase
    {
        
        private readonly IMediator mediator;

        public ObterAlternativaProvaLegadoLegadoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator)); 
        }

        
        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var busca = mensagemRabbit.ObterObjetoMensagem<BuscarPorProvaIdEQuestaoIdDto>();
            var ultimaAtualizacao = await mediator.Send(new ObterUltimoExecucaoControleTipoPorTipoQuery(ExecucaoControleTipo.ProvaLegadoSincronizacao));
            
            var alternativas = await mediator.Send(new ObterAlternativarLegadoProvaPorProvaIdQuery(busca.ProvaId, busca.QuestaoId));
            
            foreach (var alternativa in alternativas)
            {
                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.AlternativaTratar, alternativa));
            }

            await mediator.Send(new ExecucaoControleAtualizarCommand(ultimaAtualizacao));
            return true;
        }
    }
}