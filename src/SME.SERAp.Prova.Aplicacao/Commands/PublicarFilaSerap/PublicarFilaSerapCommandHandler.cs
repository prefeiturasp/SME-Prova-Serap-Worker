﻿using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class PublicarFilaSerapCommandHandler : IRequestHandler<PublicarFilaSerapCommand, bool>
    {
        private readonly IConnection connectionRabbit;
        private readonly IServicoLog servicoLog;

        public PublicarFilaSerapCommandHandler(IConnection connectionRabbit, IServicoLog servicoLog)
        {
            this.connectionRabbit = connectionRabbit ?? throw new ArgumentNullException(nameof(connectionRabbit));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public Task<bool> Handle(PublicarFilaSerapCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var mensagem = new MensagemRabbit(request.Mensagem, Guid.NewGuid());
                var body = Encoding.UTF8.GetBytes(mensagem.ConverterObjectParaJson());

                using (var canal = connectionRabbit.CreateModel())
                {
                    var props = canal.CreateBasicProperties();
                    props.Persistent = true;
                    canal.BasicPublish(ExchangeRabbit.Serap, request.Fila, props, body);
                }

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                servicoLog.Registrar($"Erros: PublicarFilaSerapCommand: Fila -> {request.Fila}", ex);
                return Task.FromResult(false);
            }
        }
    }
}
