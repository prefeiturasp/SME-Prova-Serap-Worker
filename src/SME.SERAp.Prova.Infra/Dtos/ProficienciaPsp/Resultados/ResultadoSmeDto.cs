using CsvHelper.Configuration.Attributes;
using System;

namespace SME.SERAp.Prova.Infra
{
    public class ResultadoSmeDto
    {

        [Name("Edicao")]
        public string Edicao { get; set; }

        [Name("AreaConhecimentoID")]
        public int AreaConhecimentoID { get; set; }

        [Name("AnoEscolar")]
        public string AnoEscolar { get; set; }

        [Name("Valor")]
        public string _valor { get; set; }

        [Name("TotalAlunos")]
        public int TotalAlunos { get; set; }

        [Name("NivelProficienciaID")]
        public int NivelProficienciaID { get; set; }

        [Name("PercentualAbaixoDoBasico")]
        public string _percentualAbaixoDoBasico { get; set; }

        [Name("PercentualBasico")]
        public string _percentualBasico { get; set; }

        [Name("PercentualAdequado")]
        public string _percentualAdequado { get; set; }

        [Name("PercentualAvancado")]
        public string _percentualAvancado { get; set; }


        public decimal? Valor { get { return ObterValorDecimal(_valor); } }
        public decimal? PercentualAbaixoDoBasico { get { return ObterValorDecimal(_percentualAbaixoDoBasico); } }
        public decimal? PercentualBasico { get { return ObterValorDecimal(_percentualBasico); } }
        public decimal? PercentualAdequado { get { return ObterValorDecimal(_percentualAdequado); } }
        public decimal? PercentualAvancado { get { return ObterValorDecimal(_percentualAvancado); } }

        private decimal? ObterValorDecimal(string valor)
        {
            if (string.IsNullOrEmpty(valor)) return null;
            decimal dec_valor = 0;
            if (decimal.TryParse(valor.Trim(), out dec_valor))
            {
                return dec_valor;
            }
            throw new ArgumentException($"não foi possível converter o valor para decimal: {valor}");
        }

    }
}