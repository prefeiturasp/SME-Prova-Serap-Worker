using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Channels;
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

        public async Task<bool> Handle(PublicarFilaSerapCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var mensagem = new MensagemRabbit(request.Mensagem, Guid.NewGuid());
                var body = Encoding.UTF8.GetBytes(mensagem.ConverterObjectParaJson());

                using var channel = await connectionRabbit.CreateChannelAsync();
                var props = new BasicProperties()
                {
                    Persistent = true
                };

                var address = new PublicationAddress(ExchangeType.Direct, ExchangeRabbit.Serap, request.Fila);
                await channel.BasicPublishAsync(address, props, body, cancellationToken);

                return true;
            }
            catch (Exception ex)
            {
                servicoLog.Registrar($"Erros: PublicarFilaSerapCommand: Fila -> {request.Fila}", ex);
                return false;
            }
        }
    }
}
