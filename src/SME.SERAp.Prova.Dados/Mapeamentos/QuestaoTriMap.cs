using Dapper.FluentMap.Dommel.Mapping;

namespace SME.SERAp.Prova.Dados
{
    public class QuestaoTriMap : DommelEntityMap<Dominio.QuestaoTri>
    {
        public QuestaoTriMap()
        {
            ToTable("questao_tri");

            Map(c => c.Id).ToColumn("id").IsKey();

            Map(c => c.QuestaoId).ToColumn("questao_id");
            Map(c => c.Discriminacao).ToColumn("discriminacao");
            Map(c => c.Dificuldade).ToColumn("dificuldade");
            Map(c => c.AcertoCasual).ToColumn("acerto_casual");
        }
    }
}
