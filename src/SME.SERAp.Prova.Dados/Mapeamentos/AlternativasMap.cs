using Dapper.FluentMap.Dommel.Mapping;

namespace SME.SERAp.Prova.Dados
{
    public class AlternativasMap : DommelEntityMap<Dominio.Alternativas>
    {
        public AlternativasMap()
        {
            ToTable("alternativa");
            
            Map(c => c.Id).ToColumn("id").IsKey();
            
            Map(c => c.Descricao).ToColumn("descricao");
            Map(c => c.Ordem).ToColumn("ordem");
            Map(c => c.Alternativa).ToColumn("numeracao");
            Map(c => c.QuestaoId).ToColumn("questao_id");
        }
    }
}
