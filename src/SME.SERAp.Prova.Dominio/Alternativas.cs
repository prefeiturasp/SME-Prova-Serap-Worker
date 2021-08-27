using System;

namespace SME.SERAp.Prova.Dominio
{
    public class Alternativas : EntidadeBase
    {
        public int Ordem { get; set; }
        public string Alternativa { get; set; }
        public string Descricao { get; set; }
        public long QuestaoId { get; set; }


        public Alternativas(int ordem,
            string alternativa, string descricao, long questaoId)
        {
            Ordem = ordem;
            Alternativa = alternativa;
            Descricao = descricao;
            QuestaoId = questaoId;
        }
    }
}