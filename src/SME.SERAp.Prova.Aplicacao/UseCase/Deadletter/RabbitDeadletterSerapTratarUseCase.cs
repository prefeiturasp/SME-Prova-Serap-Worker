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

        public RabbitDeadletterSerapTratarUseCase(IConnection connectionRabbit)
        {

            this.connectionRabbit = connectionRabbit ?? throw new ArgumentNullException(nameof(connectionRabbit));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var fila = mensagemRabbit.Mensagem.ToString();

            await TratarMensagens(fila);

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
                    else
                    {
                        var qntAtual = 0;

                        mensagemParaEnviar.BasicProperties.Headers.TryGetValue("x-retry", out object qntMensagem);
                        
                        IBasicProperties basicProperties = canal.CreateBasicProperties();
                        basicProperties.Persistent = true;

                        if (qntMensagem != null)
                        {
                            qntAtual = (int)qntMensagem;
                        }

                        if (qntAtual == 2)
                        {
                            await Task.Run(() => canal.BasicPublish(ExchangeRabbit.SerapEstudanteDeadLetter, $"{fila}.deadletter.final", mensagemParaEnviar.BasicProperties, mensagemParaEnviar.Body.ToArray()));
                        }
                        else
                        {
                            qntAtual += 1;

                            basicProperties.Headers = new Dictionary<string, object>();
                            basicProperties.Headers.Add("x-retry", qntAtual);                            

                            await Task.Run(() => canal.BasicPublish(ExchangeRabbit.SerapEstudante, fila, basicProperties, mensagemParaEnviar.Body.ToArray()));

                        }


                    }
                }
            }

        }
    }
}
