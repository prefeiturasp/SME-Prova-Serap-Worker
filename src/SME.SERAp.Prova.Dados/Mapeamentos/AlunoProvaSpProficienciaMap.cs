using Dapper.FluentMap.Dommel.Mapping;
using SME.SERAp.Prova.Dominio.Entidades;

namespace SME.SERAp.Prova.Dados.Mapeamentos
{
    public class AlunoProvaSpProficienciaMap : DommelEntityMap<AlunoProvaSpProficiencia>
    {
        public AlunoProvaSpProficienciaMap()
        {
            ToTable("aluno_prova_sp_proficiencia");

            Map(c => c.Id).ToColumn("id").IsKey();

            Map(c => c.AlunoRa).ToColumn("aluno_ra");
            Map(c => c.AnoLetivo).ToColumn("ano_letivo");
            Map(c => c.AnoEscolar).ToColumn("ano_escolar");
            Map(c => c.DisciplinaId).ToColumn("disciplina_id");
            Map(c => c.NivelProficiencia).ToColumn("nivel_proficiencia");
            Map(c => c.Proficiencia).ToColumn("proficiencia");
            Map(c => c.DataAtualizacao).ToColumn("data_atualizacao");
            Map(c => c.UeCodigo).ToColumn("ue_codigo");
        }
    }
}
