using System;

namespace SME.SERAp.Prova.Dominio
{
    public class Alternativas : EntidadeBase
    {
        public long ProvaLegadoId { get; set; }
        public long QuestaoLegadoId { get; set; }
        public long AlternativaLegadoId { get; set; }
        public int Ordem { get; set; }
        public string Alternativa { get; set; }
        public string Descricao { get; set; }
        public bool Correta { get; set; }
        public long QuestaoId { get; set; }
        public DateTime Inclusao { get; set; }


        public Alternativas()
        {
            Inclusao = DateTime.Now;
        }

        public Alternativas(long provaLegadoId, long questaoLegadoId, long alternativaLegadoId, int ordem,
            string alternativa, string descricao, bool correta, long questaoId)
        {
            ProvaLegadoId = provaLegadoId;
            QuestaoLegadoId = questaoLegadoId;
            AlternativaLegadoId = alternativaLegadoId;
            Ordem = ordem;
            Alternativa = alternativa;
            Descricao = descricao;
            Correta = correta;
            QuestaoId = questaoId;
            Inclusao = DateTime.Now;
        }
    }
}