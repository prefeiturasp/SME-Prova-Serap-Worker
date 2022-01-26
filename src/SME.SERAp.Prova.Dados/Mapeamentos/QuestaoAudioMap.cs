using Dapper.FluentMap.Dommel.Mapping;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Dados
{
    public class QuestaoAudioMap : DommelEntityMap<QuestaoAudio>
    {
        public QuestaoAudioMap()
        {
            ToTable("questao_audio");

            Map(c => c.Id).ToColumn("id").IsKey();

            Map(c => c.ArquivoId).ToColumn("arquivo_id");
            Map(c => c.QuestaoId).ToColumn("questao_id");
        }
    }
}
