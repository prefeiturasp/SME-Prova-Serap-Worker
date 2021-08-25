using System;

namespace SME.SERAp.Prova.Dominio
{
    public class Alternativas : EntidadeBase
    {
        public string Descricao { get; set; }
        public string Alternativa { get; set; }
        public long ItemId { get; set; }
        public long provaId { get; set; }
        public long LegadoId { get; set; }
        public int OrdermProva { get; set; }
        public int OrdermAlternativa { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
        public DateTime Inclusao { get; set; }


        public Alternativas()
        {
            Inclusao = DateTime.Now;
        }

        public Alternativas(string descricao, string alternativa, long itemId, long provaId, long legadoId,
            int ordermProva, int ordermAlternativa, DateTime inicio, DateTime fim)
        {
            Descricao = descricao;
            Alternativa = alternativa;
            ItemId = itemId;
            this.provaId = provaId;
            LegadoId = legadoId;
            OrdermProva = ordermProva;
            OrdermAlternativa = ordermAlternativa;
            Inicio = inicio;
            Fim = fim;
            Inclusao = DateTime.Now;
        }
    }
}