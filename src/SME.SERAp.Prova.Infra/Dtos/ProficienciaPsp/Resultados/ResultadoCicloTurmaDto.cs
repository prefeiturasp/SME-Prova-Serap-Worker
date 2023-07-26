using System;
using CsvHelper.Configuration.Attributes;

namespace SME.SERAp.Prova.Infra
{
    public class ResultadoCicloTurmaDto
    {
        [Name("Edicao")]
        public string Edicao { get; set; }
        
        [Name("AreaConhecimentoID")]
        public int AreaConhecimentoId { get; set; }
        
        [Name("uad_sigla")]
        public string UadSigla { get; set; }
        
        [Name("esc_codigo")]
        public string EscCodigo { get; set; }
        
        [Name("tur_codigo")]
        public string TurmaCodigo { get; set; }
        
        [Name("tur_id")]
        public string STurmaId { get; set; }        
        
        [Name("CicloID")]
        public string SCicloId { get; set; }

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

        public int? CicloId => SCicloId.ConvertStringPraIntNullPsp();
        public long? TurmaId => STurmaId.ConvertStringPraLongNullPsp();
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
                
                if (string.IsNullOrEmpty(UadSigla) || UadSigla.ToUpper().Trim() == "NA")
                    throw new Exception($"Sigla da unidade administrativa {UadSigla} inválida.");
                
                if (string.IsNullOrEmpty(EscCodigo) || EscCodigo.ToUpper().Trim() == "NA")
                    throw new Exception($"Código da escola {EscCodigo} inválido.");
                
                if (string.IsNullOrEmpty(TurmaCodigo) || TurmaCodigo.ToUpper().Trim() == "NA")
                    throw new Exception($"Código da turma {TurmaCodigo} inválido.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Dados inválidos -- {ex.Message} -- {ex.StackTrace}");
            }            
        }               
    }
}