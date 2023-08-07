using System;

namespace SME.SERAp.Prova.Dominio
{
    public class QuestaoCompleta : EntidadeBase
    {
        public QuestaoCompleta()
        {
        }

        public QuestaoCompleta(long id, long questaoLegadoId, string json, DateTime ultimaAtualizacao)
        {
            Id = id;
            QuestaoLegadoId = questaoLegadoId;  
            Json = json;
            UltimaAtualizacao = ultimaAtualizacao;
        }

        public long QuestaoLegadoId { get; set; }
        public string Json { get; set; }
        public DateTime UltimaAtualizacao { get; set; }
    }
}
