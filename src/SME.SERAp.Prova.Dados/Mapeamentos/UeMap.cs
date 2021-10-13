using Dapper.FluentMap.Dommel.Mapping;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Dados
{
    public class UeMap : DommelEntityMap<Ue>
    {
        public UeMap()
        {
            ToTable("ue");
            Map(c => c.Id).ToColumn("id").IsKey();

            Map(c => c.CodigoUe).ToColumn("ue_id");
            Map(c => c.DataAtualizacao).ToColumn("data_atualizacao");
            Map(c => c.DreId).ToColumn("dre_id");
            Map(c => c.TipoEscola).ToColumn("tipo_escola");
            Map(c => c.Nome).ToColumn("nome");
        }
    }
}
