using Polly;
using Polly.Registry;
using RabbitMQ.Client;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class RabbitDeadletterSerapTratarUseCase : IRabbitDeadletterSerapTratarUseCase
    {
        private readonly IConnection connectionRabbit;
        private readonly IAsyncPolicy policy;

        public RabbitDeadletterSerapTratarUseCase(IConnection connectionRabbit, IReadOnlyPolicyRegistry<string> registry)
        {
            this.connectionRabbit = connectionRabbit ?? throw new ArgumentNullException(nameof(connectionRabbit));
            this.policy = registry != null ? registry.Get<IAsyncPolicy>(PoliticaPolly.PublicaFila) : throw new ArgumentNullException(nameof(registry));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var fila = mensagemRabbit.Mensagem.ToString();

            await policy.ExecuteAsync(() => TratarMensagens(fila));

            return await Task.FromResult(true);
        }

        private async Task TratarMensagens(string fila)
        {
            using (IModel canal = connectionRabbit.CreateModel())
            {
                var props = canal.CreateBasicProperties();
                props.Persistent = true;

                while (true)
                {
                    var mensagemParaEnviar = canal.BasicGet($"{fila}.deadletter", true);

                    if (mensagemParaEnviar == null)
                        break;

                    mensagemParaEnviar.BasicProperties.Headers.TryGetValue("x-retry", out object qntMensagem);
                    var qntAtual = qntMensagem != null ? (int)qntMensagem : 0;

                    if (qntAtual == 2)
                    {
                        await Task.Run(() => canal.BasicPublish(ExchangeRabbit.SerapEstudanteDeadLetter, $"{fila}.deadletter.final", mensagemParaEnviar.BasicProperties, mensagemParaEnviar.Body.ToArray()));
                    }
                    else
                    {
                        qntAtual += 1;
                        IBasicProperties basicProperties = canal.CreateBasicProperties();
                        basicProperties.Persistent = true;

                        basicProperties.Headers = new Dictionary<string, object>();
                        basicProperties.Headers.Add("x-retry", qntAtual);

                        await Task.Run(() => canal.BasicPublish(ExchangeRabbit.SerapEstudante, fila, basicProperties, mensagemParaEnviar.Body.ToArray()));
                    }
                }
            }
        }
    }
}
