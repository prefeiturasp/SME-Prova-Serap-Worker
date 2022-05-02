using System;

namespace SME.SERAp.Prova.Infra
{
    public class ProvaParaAtualizarDto : DtoBase
    {
        public long ProvaId { get; set; }        
        public int Status { get; set; }
        public DateTime? CriadoEm { get; set; }
        public DateTime? FinalizadoEm { get; set; }

        public long[] IdsProvasAlunos { get; set; }
    }
}
