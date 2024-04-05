using System;

namespace SME.SERAp.Prova.Infra
{
    public class ConsolidadoAlunoProvaDto : DtoBase
    {
        public long ProvaSerapId { get; set; }
        public long ProvaSerapEstudantesId { get; set; }
        public long AlunoCodigoEol { get; set; }
        public string AlunoNome { get; set; }
        public string AlunoSexo { get; set; }
        public DateTime AlunoDataNascimento { get; set; }
        public string ProvaComponente { get; set; }
        public string ProvaCaderno { get; set; }
        public int ProvaQuantidadeQuestoes { get; set; }        
        public string AlunoFrequencia {get; set; }
        public DateTime? ProvaDataInicio { get; set; }
        public DateTime? ProvaDataEntregue { get; set; }
        public bool PossuiBib { get; set; }
        public long TurmaId { get; set; }
        public int AlunoSituacao { get; set; }
        public string DreCodigoEol { get; set; }
        public string DreSigla { get; set; }
        public string DreNome { get; set; }
        public string UeCodigoEol { get; set; }        
        public string UeNome { get; set; }
        public string TurmaAnoEscolar { get; set; }
        public string TurmaAnoEscolarDescricao { get; set; }
        public string TurmaCodigo { get; set; }
        public string TurmaDescricao { get; set; }
    }
}