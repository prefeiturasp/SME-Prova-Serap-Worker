using Dapper.FluentMap.Dommel.Mapping;

namespace SME.SERAp.Prova.Dados
{
    public class AlternativasMap : DommelEntityMap<Dominio.Alternativas>
    {
        public AlternativasMap()
        {
            ToTable("alternativa");
            
            Map(c => c.Id).ToColumn("id").IsKey();
            
            Map(c => c.Ordem).ToColumn("ordem");
            Map(c => c.ProvaLegadoId).ToColumn("prova_legado_id");
            Map(c => c.QuestaoLegadoId).ToColumn("questao_legado_id");
            Map(c => c.AlternativaLegadoId).ToColumn("alternativa_legado_id");
            Map(c => c.Alternativa).ToColumn("alternativa");
            Map(c => c.Descricao).ToColumn("descricao");
            Map(c => c.Correta).ToColumn("correta");
            Map(c => c.QuestaoId).ToColumn("questao_id");
            Map(c => c.Inclusao).ToColumn("inclusao");

        }
    }
}
