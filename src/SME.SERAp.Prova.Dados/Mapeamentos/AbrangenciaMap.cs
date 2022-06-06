using Dapper.FluentMap.Dommel.Mapping;

namespace SME.SERAp.Prova.Dados
{
    public class AbrangenciaMap : DommelEntityMap<Dominio.Abrangencia>
    {
        public AbrangenciaMap()
        {
            ToTable("abrangencia");
            Map(c => c.Id).ToColumn("id").IsKey();

            Map(c => c.UsuarioId).ToColumn("usuario_id");
            Map(c => c.GrupoId).ToColumn("grupo_id");
            Map(c => c.DreId).ToColumn("dre_id");
            Map(c => c.UeId).ToColumn("ue_id");
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.Inicio).ToColumn("inicio");
            Map(c => c.Fim).ToColumn("fim");
        }
    }
}
