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
       private readonly ConnectionFactory factory;
        
        public PublicaFilaRabbitCommandHandler(ConnectionFactory factory)
        {
            factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }
        
        public Task<bool> Handle(PublicaFilaRabbitCommand request, CancellationToken cancellationToken)
        {
            
            try
            {
                var conexaoRabbit = factory.CreateConnection();
                var model = conexaoRabbit.CreateModel();
                
                var mensagem = new MensagemRabbit(request.Mensagem, Guid.NewGuid());
        
                var mensagemJson = JsonSerializer.Serialize(mensagem);
                var body = Encoding.UTF8.GetBytes(mensagemJson);
        
                var props = model.CreateBasicProperties();
                props.Persistent = true;
        
                model.BasicPublish(ExchangeRabbit.SerapEstudante, request.NomeRota, props, body);
        
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return Task.FromResult(false);
            }
        }
    }
}