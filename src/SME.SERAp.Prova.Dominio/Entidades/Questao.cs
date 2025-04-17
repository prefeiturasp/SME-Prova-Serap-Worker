using HtmlAgilityPack;
using SME.SERAp.Prova.Dominio.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;

namespace SME.SERAp.Prova.Dominio
{
    public class Questao : EntidadeBase
    {
        public Questao()
        {
        }

        public int Ordem { get; set; }
        public int QuantidadeAlternativas { get; set; }
        public string TextoBase { get; set; }
        public string Enunciado { get; set; }
        public long QuestaoLegadoId { get; set; }
        public long ProvaId { get; set; }
        public QuestaoTipo Tipo { get; set; }
        public string Caderno { get; set; }
        public IEnumerable<Arquivo> Arquivos { get; set; }

        public long? EixoId { get; set; }
        public long? HabilidadeId { get; set; }

        public Questao(string textoBase, long questaoLegadoId, string enunciado, int ordem, long provaId, QuestaoTipo tipo, string caderno, int quantidadeAlternativas, long? eixoId = null, long? habilidadeId = null)
        {
            Ordem = ordem;
            TextoBase = textoBase;
            Enunciado = enunciado;
            QuestaoLegadoId = questaoLegadoId;
            ProvaId = provaId;
            Tipo = tipo;
            Caderno = caderno;
            QuantidadeAlternativas = quantidadeAlternativas;
            TrataArquivos();
            EixoId = eixoId;
            HabilidadeId = habilidadeId;
        }

        private void TrataArquivos()
        {
            var arquivos = new List<Arquivo>();
            var htmlDoc = new HtmlDocument();
            
            if (!string.IsNullOrEmpty(Enunciado))
            {
                htmlDoc.LoadHtml(Enunciado);
                
                arquivos.AddRange(htmlDoc.DocumentNode.Descendants("img")
                    .Select(e => new Arquivo(e.GetAttributeValue("src", string.Empty), 0, e.ObterImagemId()))
                    .Where(a => a.Caminho.Substring(0, 4).ToLower() == "http"));
            }

            if (!string.IsNullOrEmpty(TextoBase))
            {
                htmlDoc.LoadHtml(TextoBase);

                arquivos.AddRange(htmlDoc.DocumentNode.Descendants("img")
                    .Select(e => new Arquivo(e.GetAttributeValue("src", string.Empty), 0, e.ObterImagemId()))
                    .Where(a => a.Caminho.Substring(0, 4).ToLower() == "http"));
            }

            foreach (var arquivo in arquivos.Where(arquivo => !arquivo.Caminho.Contains("#")))
            {
                if(!string.IsNullOrEmpty(Enunciado))
                    Enunciado = Enunciado.Replace(arquivo.Caminho, arquivo.NovoCaminho());
                
                if(!string.IsNullOrEmpty(TextoBase))
                    TextoBase = TextoBase.Replace(arquivo.Caminho, arquivo.NovoCaminho());
            }

            Arquivos = arquivos;
        }

        public void TrataArquivosTextoBase()
        {
            var arquivos = new List<Arquivo>();
            var htmlDoc = new HtmlDocument();
           
            if (!string.IsNullOrEmpty(TextoBase))
            {
                htmlDoc.LoadHtml(TextoBase);
                arquivos.AddRange(htmlDoc.DocumentNode.Descendants("img")
                    .Select(e =>
                        new Arquivo(e.GetAttributeValue("src", string.Empty), 0, e.GetAttributeValue("id", 0))));
            }

            foreach (var arquivo in arquivos.Where(arquivo => !arquivo.Caminho.Contains("#"))
                         .Where(arquivo => !string.IsNullOrEmpty(TextoBase)))
            {
                TextoBase = TextoBase?.Replace(arquivo.Caminho, arquivo.NovoCaminho());
            }

            Arquivos = arquivos;
        }
    }
}