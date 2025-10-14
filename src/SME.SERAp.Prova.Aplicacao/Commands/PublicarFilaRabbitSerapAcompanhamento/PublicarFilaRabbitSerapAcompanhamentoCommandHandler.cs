using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Prova.Dominio.Enums;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    class PublicarFilaRabbitSerapAcompanhamentoCommandHandler : IRequestHandler<PublicarFilaRabbitSerapAcompanhamentoCommand, bool>
    {
        private readonly IChannel channel;
        private readonly IServicoLog servicoLog;

        public PublicarFilaRabbitSerapAcompanhamentoCommandHandler(IChannel channel, IServicoLog servicoLog)
        {
            this.channel = channel ?? throw new ArgumentNullException(nameof(channel));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public async Task<bool> Handle(PublicarFilaRabbitSerapAcompanhamentoCommand request, CancellationToken cancellationToken)
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

                var address = new PublicationAddress(ExchangeType.Direct, ExchangeRabbit.SerapEstudanteAcompanhamento, request.NomeRota);
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