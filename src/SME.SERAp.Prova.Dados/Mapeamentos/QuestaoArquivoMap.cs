using Dapper.FluentMap.Dommel.Mapping;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Dados
{
    public class QuestaoArquivoMap : DommelEntityMap<QuestaoArquivo>
    {
        public QuestaoArquivoMap()
        {
            ToTable("questao_arquivo");
            
            Map(c => c.Id).ToColumn("id").IsKey();
            
            Map(c => c.ArquivoId).ToColumn("arquivo_id");
            Map(c => c.QuestaoId).ToColumn("questao_id");
            
        }
    }
}
