using Dapper.FluentMap.Dommel.Mapping;

namespace SME.SERAp.Prova.Dados
{
    public class UsuarioGrupoSerapCoreSsoMap : DommelEntityMap<Dominio.UsuarioGrupoSerapCoreSso>
    {
        public UsuarioGrupoSerapCoreSsoMap()
        {
            ToTable("usuario_grupo_serap_coresso");
            Map(c => c.Id).ToColumn("id").IsKey();

            Map(c => c.IdUsuarioSerapCoreSso).ToColumn("id_usuario_serap");
            Map(c => c.IdGrupoSerapCoreSso).ToColumn("id_grupo_serap");
            Map(c => c.CriadoEm).ToColumn("criado_em");
        }
    }
}
