using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Prova.Dominio.Enums;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Interfaces;

namespace SME.SERAp.Prova.Aplicacao.Commands.FilaWorker
{
    public class PublicaFilaRabbitCommandHandler : IRequestHandler<PublicaFilaRabbitCommand, bool>
    {
        private readonly IChannel channel;
        private readonly IServicoLog servicoLog;

        public PublicaFilaRabbitCommandHandler(IChannel channel, IServicoLog servicoLog)
        {
            this.channel = channel ?? throw new ArgumentNullException(nameof(channel));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public async Task<bool> Handle(PublicaFilaRabbitCommand request, CancellationToken cancellationToken)
        {
            //TODO: Implementar Polly!
            try
            {
                var mensagem = new MensagemRabbit(request.Mensagem, Guid.NewGuid());
                var body = Encoding.UTF8.GetBytes(mensagem.ConverterObjectParaJson());
                var props = new BasicProperties()
                {
                    Persistent = true
                };

                var address = new PublicationAddress(ExchangeType.Direct, ExchangeRabbit.SerapEstudante, request.NomeRota);
                await channel.BasicPublishAsync(address, props, body, cancellationToken);

                return true;
            }
            catch (Exception ex)
            {
                servicoLog.Registrar(LogNivel.Critico, $"Erros: PublicaFilaRabbitCommand --{ex.Message}", $"Worker Serap: Rota -> {request.NomeRota} Fila -> {request.NomeFila}", ex.StackTrace);
                return false;
            }
        }
    }
}