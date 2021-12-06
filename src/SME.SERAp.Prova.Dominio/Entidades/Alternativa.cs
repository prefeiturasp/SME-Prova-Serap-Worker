using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SME.SERAp.Prova.Dominio
{
    public class Alternativa : EntidadeBase
    {
        public int Ordem { get; set; }
        public string Numeracao { get; set; }
        public string Descricao { get; set; }
        public long QuestaoId { get; set; }

        public IEnumerable<Arquivo> Arquivos { get; set; }


        public Alternativa(int ordem,
            string alternativa, string descricao, long questaoId)
        {
            Ordem = ordem;
            Numeracao = alternativa;
            Descricao = descricao;
            QuestaoId = questaoId;

            TrataArquivos();
        }

        public void TrataArquivos()
        {
            List<Arquivo> arquivos = new List<Arquivo>();
            var htmlDoc = new HtmlDocument();
            if (!string.IsNullOrEmpty(Descricao))
            {

                htmlDoc.LoadHtml(Descricao);
                arquivos.AddRange(htmlDoc.DocumentNode.Descendants("img")
                                  .Select(e => new Arquivo(e.GetAttributeValue("src", string.Empty), 0, e.GetAttributeValue("id", 0)))
                                  .Where(a => a.Caminho.Substring(0, 4).ToLower() == "http"));
            }

            foreach (var arquivo in arquivos)
            {
                if (arquivo.Caminho.Contains("#"))
                    continue;

                if (!string.IsNullOrEmpty(Descricao))
                    Descricao = Descricao.Replace(arquivo.Caminho, arquivo.NovoCaminho());
            }

            Arquivos = arquivos;
        }
    }
}