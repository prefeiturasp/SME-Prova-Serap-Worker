namespace SME.SERAp.Prova.Dominio
{
    public class QuestaoTri : EntidadeBase
    {
        public long QuestaoId { get; set; }
        public decimal Discriminacao { get; set; }
        public decimal Dificuldade { get; set; }
        public decimal AcertoCasual { get; set; }

    }
}
