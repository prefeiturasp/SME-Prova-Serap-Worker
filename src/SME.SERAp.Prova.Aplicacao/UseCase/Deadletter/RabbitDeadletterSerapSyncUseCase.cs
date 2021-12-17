using RabbitMQ.Client;
using SME.SERAp.Prova.Infra;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class RabbitDeadletterSerapSyncUseCase : IRabbitDeadletterSerapSyncUseCase
    {

        private readonly IConnection connectionRabbit;

        public RabbitDeadletterSerapSyncUseCase(IConnection connectionRabbit)
        {
            this.connectionRabbit = connectionRabbit ?? throw new ArgumentNullException(nameof(connectionRabbit));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            await EnviarParaFilaSync(mensagemRabbit);

            return await Task.FromResult(true);
        }

        private async Task EnviarParaFilaSync(MensagemRabbit mensagem)
        {
            mensagem.Mensagem = RotasRabbit.IncluirRespostaAluno;

            var mensagemJson = JsonSerializer.Serialize(mensagem);
            var body = Encoding.UTF8.GetBytes(mensagemJson);

            using IModel canal = connectionRabbit.CreateModel();

            var props = canal.CreateBasicProperties();
            props.Persistent = true;
            await Task.Run(() => canal.BasicPublish(ExchangeRabbit.SerapEstudante, RotasRabbit.FilaDeadletterTratar, props, body));

        }
    }

}
