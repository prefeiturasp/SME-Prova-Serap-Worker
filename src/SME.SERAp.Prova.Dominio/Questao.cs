using System;

namespace SME.SERAp.Prova.Dominio
{
    public class Questao : EntidadeBase
    {
        public int Ordem { get; set; }
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

        public Questao(int ordem, string pergunta, string enunciado, long provaLegadoId, long questaoLegadoId, long provaId)
        {
            Ordem = ordem;
            Pergunta = pergunta;
            Enunciado = enunciado;
            ProvaLegadoId = provaLegadoId;
            QuestaoLegadoId = questaoLegadoId;
            ProvaId = provaId;
            Inclusao = DateTime.Now;
        }
    }
}