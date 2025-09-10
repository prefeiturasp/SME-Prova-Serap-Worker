using Dapper.FluentMap.Dommel.Mapping;
using SME.SERAp.Prova.Dominio.Entidades;

namespace SME.SERAp.Prova.Dados.Mapeamentos
{
    public class BoletimProvaAlunoMap : DommelEntityMap<BoletimProvaAluno>
    {
        public BoletimProvaAlunoMap()
        {
            ToTable("boletim_prova_aluno");

            Map(c => c.Id).ToColumn("id").IsKey();

            Map(c => c.DreId).ToColumn("dre_id");
            Map(c => c.CodigoUe).ToColumn("ue_codigo");
            Map(c => c.NomeUe).ToColumn("ue_nome");
            Map(c => c.ProvaId).ToColumn("prova_id");
            Map(c => c.ProvaDescricao).ToColumn("prova_descricao");
            Map(c => c.AnoEscolar).ToColumn("ano_escolar");
            Map(c => c.Turma).ToColumn("turma");
            Map(c => c.AlunoRa).ToColumn("aluno_ra");
            Map(c => c.AlunoNome).ToColumn("aluno_nome");
            Map(c => c.Disciplina).ToColumn("disciplina");
            Map(c => c.DisciplinaId).ToColumn("disciplina_id");
            Map(c => c.ProvaStatus).ToColumn("status_prova");
            Map(c => c.Proficiencia).ToColumn("proficiencia");
            Map(c => c.ErroMedida).ToColumn("erro_medida");
            Map(c => c.NivelCodigo).ToColumn("nivel_codigo");
        }
    }
}
