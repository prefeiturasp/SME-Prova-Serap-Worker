using SME.SERAp.Prova.Dominio.Enums;
using System;

namespace SME.SERAp.Prova.Dominio
{
    public class LogMensagem
    {
        public LogMensagem(string mensagem, LogNivel nivel, string observacao, string rastreamento = null, string excecaoInterna = null, string projeto = "Serap-Prova-Worker")
        {
            Mensagem = mensagem;
            Nivel = nivel;
            Observacao = observacao;
            Projeto = projeto;
            Rastreamento = rastreamento;
            ExcecaoInterna = excecaoInterna;
            DataHora = DateTime.Now;
        }

        public string Mensagem { get; set; }
        public LogNivel Nivel { get; set; }
        public string Observacao { get; set; }
        public string Projeto { get; set; }
        public string Rastreamento { get; set; }
        public string ExcecaoInterna { get; set; }
        public DateTime DataHora { get; set; }

    }
}
