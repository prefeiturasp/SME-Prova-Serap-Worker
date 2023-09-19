
namespace SME.SERAp.Prova.Dominio.Entidades.ProficienciaPsp
{
    public class ResultadoCicloDre
    {
        public string Edicao { get; set; }
        public int AreaConhecimentoId { get; set; }
        public string  DreSigla { get; set; }
        public int CicloId { get; set; }
        public decimal? Valor { get; set; }
        public int? TotalAlunos { get; set; }
        public int? NivelProficienciaId { get; set; }
        public decimal? PercentualAbaixoDoBasico { get; set; }
        public decimal? PercentualBasico { get; set; }
        public decimal? PercentualAdequado { get; set; }
        public decimal? PercentualAvancado { get; set; }
        public decimal? PercentualAlfabetizado { get; set; }
    }
}
