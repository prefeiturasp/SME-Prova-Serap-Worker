using System;
using System.Text;
using System.Threading.Tasks;
using Polly;
using Polly.Registry;
using RabbitMQ.Client;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using SME.SERAp.Prova.Infra.Interfaces;

namespace SME.SERAp.Prova.Infra.Services
{
    public class ServicoMensageria : IServicoMensageria
    {
        private readonly RabbitOptions rabbitOptions;
        private readonly IServicoTelemetria servicoTelemetria;
        private readonly IAsyncPolicy policy;

        public ServicoMensageria(RabbitOptions rabbitOptions,
            IServicoTelemetria servicoTelemetria,
            IReadOnlyPolicyRegistry<string> registry)
        {
            this.rabbitOptions = rabbitOptions ?? throw new ArgumentNullException(nameof(rabbitOptions));
            this.servicoTelemetria = servicoTelemetria ?? throw new ArgumentNullException(nameof(servicoTelemetria));
            policy = registry.Get<IAsyncPolicy>(PoliticaPolly.PublicaFila);
        }

        public async Task<bool> Publicar(MensagemRabbit mensagemRabbit, string rota, string exchange, string nomeAcao)
        {
            var body = Encoding.UTF8.GetBytes(mensagemRabbit.ConverterObjectParaJson());
            
            await servicoTelemetria.RegistrarAsync(
                async () => await policy.ExecuteAsync(async () =>
                    await PublicarMensagem(rota, body, exchange)), nomeAcao, rota, string.Empty);

            return true;
        }

        private async Task PublicarMensagem(string rota, byte[] body, string exchange = null)
        {
            var factory = new ConnectionFactory
            {
                HostName = rabbitOptions.HostName,
                UserName = rabbitOptions.UserName,
                Password = rabbitOptions.Password,
                VirtualHost = rabbitOptions.VirtualHost
            };

            using var conexaoRabbit = await factory.CreateConnectionAsync();
            using var channel = await conexaoRabbit.CreateChannelAsync();
            var props = new BasicProperties
            {
                Persistent = true
            };

            await channel.BasicPublishAsync(
                ExchangeRabbit.Logs,
                RotasRabbit.RotaLogs,
                true,
                props,
                body
            );
        }
    }
}