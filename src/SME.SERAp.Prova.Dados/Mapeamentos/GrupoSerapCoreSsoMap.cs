using Dapper.FluentMap.Dommel.Mapping;

namespace SME.SERAp.Prova.Dados
{
    public class GrupoSerapCoreSsoMap : DommelEntityMap<Dominio.GrupoSerapCoreSso>
    {
        public GrupoSerapCoreSsoMap()
        {
            ToTable("grupo_serap_coresso");
            Map(c => c.Id).ToColumn("id").IsKey();
            Map(c => c.IdCoreSso).ToColumn("id_coresso");
            Map(c => c.Nome).ToColumn("nome");
            Map(c => c.CriadoEm).ToColumn("criado_em");
        }
    }
}
