using Dapper.FluentMap.Dommel.Mapping;

namespace SME.SERAp.Prova.Dados
{
    public class AlunoDeficienciaMap : DommelEntityMap<Dominio.AlunoDeficiencia>
    {
        public AlunoDeficienciaMap()
        {
            ToTable("aluno_deficiencia");

            Map(c => c.Id).ToColumn("id").IsKey();

            Map(c => c.DeficienciaId).ToColumn("deficiencia_id");
            Map(c => c.AlunoRa).ToColumn("aluno_ra");
            Map(c => c.CriadoEm).ToColumn("criado_em");
            Map(c => c.AtualizadoEm).ToColumn("atualizado_em");
        }
    }
}
