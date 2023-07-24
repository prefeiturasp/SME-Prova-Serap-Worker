using System;
using CsvHelper.Configuration.Attributes;

namespace SME.SERAp.Prova.Infra
{
    public class ResultadoCicloSmeDto
    {
        [Name("Edicao")]
        public string Edicao { get; set; }
        
        [Name("AreaConhecimentoID")]
        public int AreaConhecimentoId { get; set; }
        
        [Name("CicloID")]
        public int CicloId { get; set; }
        
        [Name("Valor")]
        public string SValor { get; set; }

        [Name("TotalAlunos")]
        public string STotalAlunos { get; set; }
        
        [Name("NivelProficienciaID")]
        public string SNivelProficienciaId { get; set; }
        
        [Name("PercentualAbaixoDoBasico")]
        public string SPercentualAbaixoDoBasico { get; set; }
        
        [Name("PercentualBasico")]
        public string SPercentualBasico { get; set; }
        
        [Name("PercentualAdequado")]
        public string SPercentualAdequado { get; set; }
        
        [Name("PercentualAvancado")]
        public string SPercentualAvancado { get; set; }
        
        [Name("PercentualAlfabetizado")]
        public string SPercentualAlfabetizado { get; set; }

        public int? NivelProficienciaId => SNivelProficienciaId.ConvertStringPraIntNullPsp();
        public decimal? Valor => SValor.ConvertStringPraDecimalNullPsp();
        public int? TotalAlunos => STotalAlunos.ConvertStringPraIntNullPsp();
        public decimal? PercentualAbaixoDoBasico => SPercentualAbaixoDoBasico.ConvertStringPraDecimalNullPsp();
        public decimal? PercentualBasico => SPercentualBasico.ConvertStringPraDecimalNullPsp();
        public decimal? PercentualAdequado => SPercentualAdequado.ConvertStringPraDecimalNullPsp();
        public decimal? PercentualAvancado => SPercentualAvancado.ConvertStringPraDecimalNullPsp();
        public decimal? PercentualAlfabetizado => SPercentualAlfabetizado.ConvertStringPraDecimalNullPsp();

        public void ValidarCampos()
        {
            try
            {
                ResultadoPsp.ValidarAreaConhecimentoId(AreaConhecimentoId);
                
                if (CicloId <= 0)
                    throw new Exception($"Ciclo {CicloId} inválido.");                
            }
            catch (Exception ex)
            {
                throw new Exception($"Dados inválidos -- {ex.Message} -- {ex.StackTrace}");
            }            
        }
    }
}