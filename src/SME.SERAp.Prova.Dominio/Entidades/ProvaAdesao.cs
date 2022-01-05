using System;

namespace SME.SERAp.Prova.Dominio
{
    public class ProvaAdesao : EntidadeBase
    {
        public ProvaAdesao()
        {

        }

        public ProvaAdesao(long provaId, long ueId, long turmaId, long alunoId)
        {
            ProvaId = provaId;
            UeId = ueId;
            TurmaId = turmaId;
            AlunoId = alunoId;
            CriadoEm = DateTime.Now;
            AtualizadoEm = DateTime.Now;
        }

        public long ProvaId { get; set; }
        public long UeId { get; set; }
        public long TurmaId { get; set; }
        public long AlunoId { get; set; }
        public long AlunoRa { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime AtualizadoEm { get; set; }
    }
}
