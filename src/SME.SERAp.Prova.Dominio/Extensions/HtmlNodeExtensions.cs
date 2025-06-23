using HtmlAgilityPack;
using System.Linq;

namespace SME.SERAp.Prova.Dominio.Extensions
{
    public static class HtmlNodeExtensions
    {
        public static int ObterImagemId(this HtmlNode node)
        {
            var id = node.GetAttributeValue("id", 0);
            if (id == 0)
            {
                var valorDataFileName = node.GetAttributeValue("data-filename", string.Empty);
                if (!string.IsNullOrWhiteSpace(valorDataFileName))
                {
                    var valorIdString = valorDataFileName.Split('.')?.FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(valorIdString))
                    {
                        if(int.TryParse(valorIdString, out var valorId))
                        {
                            return valorId;
                        }
                    }
                }
            }
                
            return id;
        }
    }
}
