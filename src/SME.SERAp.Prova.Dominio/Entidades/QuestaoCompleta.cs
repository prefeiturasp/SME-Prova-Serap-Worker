using System;

namespace SME.SERAp.Prova.Dominio
{
    public class QuestaoCompleta : EntidadeBase
    {
        public QuestaoCompleta(long id, string json, DateTime ultimaAtualizacao)
        {
            Id = id;
            Json = json;
            UltimaAtualizacao = ultimaAtualizacao;
        }

        public string Json { get; set; }
        public DateTime UltimaAtualizacao { get; set; }
    }
}
