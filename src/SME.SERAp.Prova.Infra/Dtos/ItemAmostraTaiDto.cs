
namespace SME.SERAp.Prova.Infra
{
    public class ItemAmostraTaiDto
    {
        public long ItemId { get; set; }
        public string ItemCodigo { get; set; }
        public long TipoCurriculoGradeId { get; set; }
        public long HabilidadeId { get; set; }
        public string HabilidadeNome { get; set; }
        public string HabilidadeCodigo { get; set; }
        public long AssuntoId { get; set; }
        public string AssuntoNome { get; set; }
        public long SubAssuntoId { get; set; }
        public string SubAssuntoNome { get; set; }        
        public decimal Discriminacao { get; set; }
        public decimal ProporcaoAcertos { get; set; }
        public decimal AcertoCasual { get; set; }
    }
}
