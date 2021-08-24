using Dapper.FluentMap.Dommel.Mapping;

namespace SME.SERAp.Prova.Dados
{
    public class QuestaoMap : DommelEntityMap<Dominio.Questao>
    {
        public QuestaoMap()
        {
            ToTable("questao");
            
            Map(c => c.Id).ToColumn("id").IsKey();
            
            Map(c => c.Ordem).ToColumn("ordem");
            Map(c => c.Pergunta).ToColumn("pergunta");
            Map(c => c.Enunciado).ToColumn("enunciado");
            Map(c => c.ProvaLegadoId).ToColumn("prova_legado_id");
            Map(c => c.QuestaoLegadoId).ToColumn("questao_legado_id");
            Map(c => c.ProvaId).ToColumn("prova_id");
            Map(c => c.Inclusao).ToColumn("inclusao");

        }
    }
}
