
namespace SME.SERAp.Prova.Dominio
{
    public class ParticipacaoTurma
    {
        public string Edicao { get; set; }
        public string UadSigla { get; set; }
        public string EscCodigo { get; set; }
        public string AnoEscolar { get; set; }
        public string TurCodigo { get; set; }
        public long TurId { get; set; }
        public int TotalPrevisto { get; set; }
        public int TotalPresente { get; set; }
        public decimal PercentualParticipacao { get; set; }
    }
}
