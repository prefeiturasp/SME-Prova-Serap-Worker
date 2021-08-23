using System;

namespace SME.SERAp.Prova.Dominio
{
    public class Questao : EntidadeBase
    {
        public int Orderm { get; set; }
        public string Pergunta { get; set; }
        public string Enunciado { get; set; }
        public long ProvaLegadoId { get; set; }
        public long QuestaoLegadoId { get; set; }
        public long ProvaId { get; set; }
        public DateTime Inclusao { get; set; }


        public Questao()
        {
            Inclusao = DateTime.Now;
        }

        public Questao(int orderm, string pergunta, string enunciado, long provaLegadoId, long questaoLegadoId, long provaId)
        {
            Orderm = orderm;
            Pergunta = pergunta;
            Enunciado = enunciado;
            ProvaLegadoId = provaLegadoId;
            QuestaoLegadoId = questaoLegadoId;
            ProvaId = provaId;
            Inclusao = DateTime.Now;
        }
    }
}