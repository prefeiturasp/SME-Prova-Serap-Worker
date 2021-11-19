using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;

namespace SME.SERAp.Prova.Dominio
{
    public class Questao : EntidadeBase
    {
        public int Ordem { get; set; }
        public int QuantidadeAlternativas { get; set; }
        public string Pergunta { get; set; }
        public string Enunciado { get; set; }
        public long QuestaoLegadoId { get; set; }
        public long ProvaId { get; set; }
        public QuestaoTipo Tipo { get; set; }
        public string Caderno { get; set; }
        public IEnumerable<Arquivo> Arquivos { get; set; }


        public Questao()
        {
        }

        public Questao(string pergunta, long questaoLegadoId, string enunciado, int ordem, long provaId, QuestaoTipo tipo, string caderno, int quantidadeAlternativas)
        {
            Ordem = ordem;
            Pergunta = pergunta;
            Enunciado = enunciado;
            QuestaoLegadoId = questaoLegadoId;
            ProvaId = provaId;
            Tipo = tipo;
            Caderno = caderno;
            QuantidadeAlternativas = quantidadeAlternativas;

            TrataArquivosDaPergunta();
        }

        private void TrataArquivosDaPergunta()
        {
            if (!string.IsNullOrEmpty(Enunciado))
            {
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(Enunciado);

                Arquivos = htmlDoc.DocumentNode.Descendants("img")
                                  .Select(e => new Arquivo(e.GetAttributeValue("src", string.Empty), 0, e.GetAttributeValue("id", 0)));

                foreach (var arquivo in Arquivos)
                {
                    Enunciado = Enunciado.Replace(arquivo.Caminho, arquivo.CaminhoParaEnunciado());
                }
            }
        }
    }
}