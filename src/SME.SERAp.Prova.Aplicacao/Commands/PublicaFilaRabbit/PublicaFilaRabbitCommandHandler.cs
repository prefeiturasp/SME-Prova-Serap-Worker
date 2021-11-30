using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using RabbitMQ.Client;
using Sentry;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao.Commands.FilaWorker
{
    public class PublicaFilaRabbitCommandHandler : IRequestHandler<PublicaFilaRabbitCommand, bool>
    {
        private readonly IModel model;
        private readonly ConnectionFactory factory;

        public PublicaFilaRabbitCommandHandler(IModel model, ConnectionFactory factory)
        {
            this.model = model ?? throw new ArgumentNullException(nameof(model));
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public Task<bool> Handle(PublicaFilaRabbitCommand request, CancellationToken cancellationToken)
        {
            //TODO: Implementar Polly!
            try
            {
                var mensagem = new MensagemRabbit(request.Mensagem, Guid.NewGuid());

                var mensagemJson = JsonSerializer.Serialize(mensagem);
                var body = Encoding.UTF8.GetBytes(mensagemJson);

                var props = model.CreateBasicProperties();
                props.Persistent = true;
                
                Console.WriteLine($"HostName: {factory.HostName}");
                Console.WriteLine($"Usuário: {factory.UserName}");
                Console.WriteLine($"VirtualHost: {factory.VirtualHost}");

                model.BasicPublish(ExchangeRabbit.SerapEstudante, request.NomeRota, props, body);

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                SentrySdk.AddBreadcrumb($"Erros: PublicaFilaRabbitCommand", null, null, null, BreadcrumbLevel.Error);
                SentrySdk.CaptureMessage($"Worker Serap: Rota -> {request.NomeRota} Fila -> {request.NomeFila}", SentryLevel.Error);
                SentrySdk.CaptureException(ex);
                return Task.FromResult(false);
            }
        }
    }
}