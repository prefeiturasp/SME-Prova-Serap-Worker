﻿namespace SME.SERAp.Prova.Infra
{
    public class AlunoCadernoProvaTaiTratarDto : DtoBase
    {
        public AlunoCadernoProvaTaiTratarDto(long provaId, long alunoId, long provaLegadoId, long alunoRa)
        {
            ProvaId = provaId;
            AlunoId = alunoId;
            ProvaLegadoId = provaLegadoId;
            AlunoRa = alunoRa;
        }

        public long ProvaId { get; }
        public long AlunoId { get; }
        public long ProvaLegadoId { get; }
        public long AlunoRa { get; }
    }
}
