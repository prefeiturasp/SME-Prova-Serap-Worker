using Dapper.FluentMap.Dommel.Mapping;

namespace SME.SERAp.Prova.Dados.Mapeamentos
{
    public class ProvaAlunoReaberturaMap : DommelEntityMap<Dominio.ProvaAlunoReabertura>
    {
        public ProvaAlunoReaberturaMap()
        {
            ToTable("prova_aluno_reabertura");

            Map(c => c.Id).ToColumn("id").IsKey();
            Map(c => c.ProvaId).ToColumn("prova_id");
            Map(c => c.AlunoRA).ToColumn("aluno_ra");
            Map(c => c.LoginCoresso).ToColumn("login_coresso");
            Map(c => c.UsuarioCoresso).ToColumn("usuario_id_coresso");
            Map(c => c.GrupoCoresso).ToColumn("grupo_coresso");
            Map(c => c.CriadoEm).ToColumn("criado_em");
            Map(c => c.AlteradoEm).ToColumn("alterado_em");
        }
    }
}
