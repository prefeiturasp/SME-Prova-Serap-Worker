using System;

namespace SME.SERAp.Prova.Infra
{
    public class MensagemRabbit
    {
        public MensagemRabbit(string action, object mensagem, Guid codigoCorrelacao)
        {
            Action = action;
            Mensagem = mensagem;
            CodigoCorrelacao = codigoCorrelacao;
        }

        public MensagemRabbit(object mensagem, Guid codigoCorrelacao)
        {
            Mensagem = mensagem;
            CodigoCorrelacao = codigoCorrelacao;            
        }

        public MensagemRabbit(object mensagem)
        {
            Mensagem = mensagem;
        }

        protected MensagemRabbit()
        {

        }
        public string Action { get; set; }
        public object Mensagem { get; set; }
        public Guid CodigoCorrelacao { get; set; }     

        public T ObterObjetoMensagem<T>() where T : class
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(Mensagem.ToString());
        }
    }
}
