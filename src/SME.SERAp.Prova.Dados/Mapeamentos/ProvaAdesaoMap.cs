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
            Map(c => c.TurmaId).ToColumn("turma_id");
            Map(c => c.AlunoId).ToColumn("aluno_id");
            Map(c => c.CriadoEm).ToColumn("criado_em");
            Map(c => c.AtualizadoEm).ToColumn("atualizado_em");

        }
    }
}
