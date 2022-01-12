using System;

namespace SME.SERAp.Prova.Dominio
{
    public class ProvaAdesao : EntidadeBase
    {
        public ProvaAdesao()
        {

        }

        public ProvaAdesao(long provaId, long ueId, long alunoRa, string anoTurma, int tipoTurma, int modalidade, int tipoturno)
        {
            ProvaId = provaId;
            UeId = ueId;
            AlunoRa = alunoRa;
            CriadoEm = DateTime.Now;
            AtualizadoEm = DateTime.Now;
            AnoTurma = anoTurma;
            TipoTurma = tipoTurma;
            Modalidade = modalidade;
            Tipoturno = tipoturno;
        }

        public long ProvaId { get; set; }
        public long UeId { get; set; }
        public long AlunoRa { get; set; }
        public string AnoTurma { get; set; }
        public int TipoTurma { get; set; }
        public int Modalidade { get; set; }
        public int Tipoturno { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime AtualizadoEm { get; set; }

    }
}
