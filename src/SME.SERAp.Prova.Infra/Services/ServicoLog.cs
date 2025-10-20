using RabbitMQ.Client;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Dominio.Enums;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Infra.Services
{
    public class ServicoLog : IServicoLog
    {
        private readonly IServicoTelemetria servicoTelemetria;
        private readonly RabbitLogOptions configuracaoRabbitOptions;

        public ServicoLog(IServicoTelemetria servicoTelemetria, RabbitLogOptions configuracaoRabbitOptions)
        {
            this.servicoTelemetria = servicoTelemetria ?? throw new ArgumentNullException(nameof(servicoTelemetria));
            this.configuracaoRabbitOptions = configuracaoRabbitOptions ?? throw new System.ArgumentNullException(nameof(configuracaoRabbitOptions));
        }

        public void Registrar(Exception ex)
        {
            var logMensagem = new LogMensagem("Exception --- ", LogNivel.Critico, ex.Message, ex.StackTrace);
            Registrar(logMensagem);
        }

        public void Registrar(LogNivel nivel, string erro, string observacoes, string stackTrace)
        {
            var logMensagem = new LogMensagem(erro, nivel, observacoes, stackTrace);
            Registrar(logMensagem);
        }

        public void Registrar(LogNivel nivel, string mensagem)
        {
            var logMensagem = new LogMensagem(mensagem, nivel, "");
            Registrar(logMensagem);
        }

        public void Registrar(string mensagem, Exception ex)
        {
            var logMensagem = new LogMensagem(mensagem, LogNivel.Critico, ex.Message, ex.StackTrace);
            Registrar(logMensagem);
        }
        
        private void Registrar(LogMensagem log)
        {
            var body = Encoding.UTF8.GetBytes(log.ConverterObjectParaJson());
            servicoTelemetria.Registrar(() => PublicarMensagem(body), "RabbitMQ", "Salvar Log Via Rabbit", RotasRabbit.RotaLogs);
        }

        private async void PublicarMensagem(byte[] body)
        {
            var factory = new ConnectionFactory
            {
                HostName = configuracaoRabbitOptions.HostName,
                UserName = configuracaoRabbitOptions.UserName,
                Password = configuracaoRabbitOptions.Password,
                VirtualHost = configuracaoRabbitOptions.VirtualHost
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


