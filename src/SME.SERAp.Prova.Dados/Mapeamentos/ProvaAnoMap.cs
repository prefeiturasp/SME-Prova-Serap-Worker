using Dapper.FluentMap.Dommel.Mapping;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Dados
{
    public class ProvaAnoMap : DommelEntityMap<ProvaAno>
    {
        public ProvaAnoMap()
        {
            ToTable("prova_ano");
            
            Map(c => c.Id).ToColumn("id").IsKey();
            
            Map(c => c.ProvaId).ToColumn("prova_id");
            Map(c => c.Ano).ToColumn("ano");

        }
    }
}
