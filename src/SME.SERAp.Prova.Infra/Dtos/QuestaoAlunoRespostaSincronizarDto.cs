namespace SME.SERAp.Prova.Infra
{
    public class QuestaoAlunoRespostaSincronizarDto : DtoBase
    {
        public long AlunoRa { get; set; }
        public long QuestaoId { get; set; }
        public long? AlternativaId { get; set; }
        public string Resposta { get; set; }
        public long DataHoraRespostaTicks { get; set; }
        public int? TempoRespostaAluno { get; set; }
    }
}