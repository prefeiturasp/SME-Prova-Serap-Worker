using Dapper.FluentMap.Dommel.Mapping;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Dados
{
    public class DreMap : DommelEntityMap<Dre>
    {
        public DreMap()
        {
            ToTable("dre");

            Map(c => c.Id).ToColumn("id").IsKey();

            Map(c => c.Abreviacao).ToColumn("abreviacao");
            Map(c => c.CodigoDre).ToColumn("dre_id");
            Map(c => c.DataAtualizacao).ToColumn("data_atualizacao");            
            Map(c => c.Nome).ToColumn("nome");
        }
    }
}
