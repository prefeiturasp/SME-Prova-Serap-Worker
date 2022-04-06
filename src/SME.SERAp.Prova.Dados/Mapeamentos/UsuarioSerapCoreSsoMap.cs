using Dapper.FluentMap.Dommel.Mapping;

namespace SME.SERAp.Prova.Dados
{
    public class UsuarioSerapCoreSsoMap : DommelEntityMap<Dominio.UsuarioSerapCoreSso>
    {
        public UsuarioSerapCoreSsoMap()
        {
            ToTable("usuario_serap_coresso");
            Map(c => c.Id).ToColumn("id").IsKey();
            Map(c => c.IdCoreSso).ToColumn("id_coresso");
            Map(c => c.Login).ToColumn("login");
            Map(c => c.Nome).ToColumn("nome");            
            Map(c => c.CriadoEm).ToColumn("criado_em");
            Map(c => c.AtualizadoEm).ToColumn("atualizado_em");
        }
    }
}
