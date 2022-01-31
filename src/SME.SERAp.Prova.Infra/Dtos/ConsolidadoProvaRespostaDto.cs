using System;

namespace SME.SERAp.Prova.Infra
{
    public class ConsolidadoProvaRespostaDto
    {
        public long ProvaSerapId { get; set; }
		public long ProvaSerapEstudantesId { get; set; }
		public string DreCodigoEol { get; set; }
		public string DreSigla { get; set; }
		public string DreNome { get; set; }
		public string UeCodigoEol { get; set; }
		public string UeNome { get; set; }
		public string TurmaAnoEscolar { get; set; }
		public string TurmaAnoEscolarDescricao { get; set; }
		public string TurmaCodigo { get; set; }
		public string TurmaDescricao { get; set; }
		public int ProvaQuantidadeQuestoes { get; set; }
		public long AlunoCodigoEol { get; set; }
		public string AlunoNome { get; set; }
		public string AlunoSexo { get; set; }
		public DateTime AlunoDataNascimento { get; set; }
		public string ProvaComponente { get; set; }
		public string ProvaCaderno { get; set; }
		public int TempoTotalProva { get; set; }
		public string AlunoFrequencia {get; set; }
		public DateTime? DataInicio { get; set; }
		public DateTime? DataFim { get; set; }
		public long QuestaoId { get; set; }
		public int QuestaoOrdem { get; set; }
		public string Resposta { get; set; }		

		public ConsolidadoProvaRespostaDto()
        {
        }

		public void CalcularTempoTotalProva()
        {
			if (DataInicio != null && DataFim != null)
            {
				TimeSpan tempoTotal = (DateTime)DataFim - (DateTime)DataInicio;
				TempoTotalProva = (int)tempoTotal.TotalSeconds;
			}
        }
    }
}