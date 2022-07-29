using Dapper.FluentMap.Dommel.Mapping;

namespace SME.SERAp.Prova.Dados
{
    public class QuestaoCompletaMap : DommelEntityMap<Dominio.QuestaoCompleta>
    {
        public QuestaoCompletaMap()
        {
            ToTable("questao_completa");

            Map(c => c.Id).ToColumn("id").IsKey();
            Map(c => c.QuestaoLegadoId).ToColumn("questao_legado_id");
            Map(c => c.Json).ToColumn("json");
            Map(c => c.UltimaAtualizacao).ToColumn("ultima_atualizacao");
        }
    }
}
