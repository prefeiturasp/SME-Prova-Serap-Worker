using System;

namespace SME.SERAp.Prova.Infra.Dtos
{
    public class QuestaoAlunoRespostaComOrdemDto
    {
        public long QuestaoId { get; set; }
        public long AlunoRa { get; set; }
        public long? AlternativaId { get; set; }
        public DateTime CriadoEm { get; set; }
        public string DispositivoId { get; set; }
        public int Ordem { get; set; }
    }
}
