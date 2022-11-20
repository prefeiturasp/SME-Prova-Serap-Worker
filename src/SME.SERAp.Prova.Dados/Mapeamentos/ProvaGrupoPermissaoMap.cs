using Dapper.FluentMap.Dommel.Mapping;

namespace SME.SERAp.Prova.Dados
{
    public class ProvaGrupoPermissaoMap : DommelEntityMap<Dominio.ProvaGrupoPermissao>
    {
        public ProvaGrupoPermissaoMap()
        {
            ToTable("prova_grupo_permissao");

            Map(c => c.Id).ToColumn("id").IsKey();

            Map(c => c.ProvaId).ToColumn("prova_id");
            Map(c => c.ProvaLegadoId).ToColumn("prova_legado_id");
            Map(c => c.GrupoId).ToColumn("grupo_id");
            Map(c => c.OcultarProva).ToColumn("ocultar_prova");
            Map(c => c.CriadoEm).ToColumn("criado_em");
            Map(c => c.AlteradoEm).ToColumn("alterado_em");
        }
    }
}
