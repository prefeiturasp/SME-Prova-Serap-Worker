using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Prova.Dominio.Enums;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    class PublicarFilaRabbitSerapAcompanhamentoCommandHandler : IRequestHandler<PublicarFilaRabbitSerapAcompanhamentoCommand, bool>
    {
        private readonly IModel model;
        private readonly IServicoLog servicoLog;

        public PublicarFilaRabbitSerapAcompanhamentoCommandHandler(IModel model, IServicoLog servicoLog)
        {
            this.model = model ?? throw new ArgumentNullException(nameof(model));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public Task<bool> Handle(PublicarFilaRabbitSerapAcompanhamentoCommand request, CancellationToken cancellationToken)
        {
            //TODO: Implementar Polly!
            try
            {
                var mensagem = new MensagemRabbit(request.Mensagem, Guid.NewGuid());
                var body = Encoding.UTF8.GetBytes(mensagem.ConverterObjectParaJson());

                var props = model.CreateBasicProperties();
                props.Persistent = true;

                model.BasicPublish(ExchangeRabbit.SerapEstudanteAcompanhamento, request.NomeRota, props, body);

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                servicoLog.Registrar(LogNivel.Critico, $"Erros: PublicaFilaRabbitCommand --{ex.Message}", $"Worker Serap: Rota -> {request.NomeRota} Fila -> {request.NomeFila}", ex.StackTrace);
                return Task.FromResult(false);
            }
        }
    }
}