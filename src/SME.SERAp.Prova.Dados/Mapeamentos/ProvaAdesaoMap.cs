using Dapper.FluentMap.Dommel.Mapping;

namespace SME.SERAp.Prova.Dados
{
    public class ProvaAdesaoMap : DommelEntityMap<Dominio.ProvaAdesao>
    {
        public ProvaAdesaoMap()
        {
            ToTable("prova_adesao");

            Map(c => c.Id).ToColumn("id").IsKey();

            Map(c => c.ProvaId).ToColumn("prova_id");
            Map(c => c.UeId).ToColumn("ue_id");
            Map(c => c.AlunoRa).ToColumn("aluno_ra");
            Map(c => c.AnoTurma).ToColumn("ano_turma");
            Map(c => c.TipoTurma).ToColumn("tipo_turma");
            Map(c => c.Modalidade).ToColumn("modalidade_codigo");
            Map(c => c.Tipoturno).ToColumn("tipo_turno");
            Map(c => c.CriadoEm).ToColumn("criado_em");
            Map(c => c.AtualizadoEm).ToColumn("atualizado_em");

        }
    }
}
