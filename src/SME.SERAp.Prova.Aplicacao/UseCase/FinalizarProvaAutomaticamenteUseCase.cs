﻿using MediatR;
using Sentry;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class FinalizarProvaAutomaticamenteUseCase : IFinalizarProvaAutomaticamenteUseCase
    {
        private readonly IMediator mediator;
        public FinalizarProvaAutomaticamenteUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var provaParaFinalizar = mensagemRabbit.ObterObjetoMensagem<ProvaParaAtualizarDto>();

            await mediator.Send(new FinalizarProvaAutomaticamenteCommand(provaParaFinalizar));
            return true;
        }
    }
}
