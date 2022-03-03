using System;

namespace SME.SERAp.Prova.Dominio
{
    public class AlunoDeficiencia : EntidadeBase
    {
        public AlunoDeficiencia()
        {

        }

        public AlunoDeficiencia(long deficienciaId, long alunoRa)
        {
            DeficienciaId = deficienciaId;
            AlunoRa = alunoRa;
            CriadoEm = AtualizadoEm = DateTime.Now;
        }

        public long DeficienciaId { get; set; }
        public long AlunoRa { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime AtualizadoEm { get; set; }
    }
}
