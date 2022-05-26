using System;

namespace SME.SERAp.Prova.Infra
{
    public class QuestaoAtualizada : DtoBase
    {
        public long Id { get; set; }
        public DateTime UltimaAtualizacao { get; set; }
    }
}
