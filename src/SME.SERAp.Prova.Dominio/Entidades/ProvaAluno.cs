using System;

namespace SME.SERAp.Prova.Dominio
{
    public class ProvaAluno : EntidadeBase
    {
        public ProvaAluno()
        {
            CriadoEm = DateTime.Now;
        }

        public long ProvaId { get; set; }
        public long AlunoRA { get; set; }
        public ProvaStatus Status { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime? FinalizadoEm { get; set; }
        public FrequenciaAluno Frequencia { get; set; }
        public TipoDispositivo TipoDispositivo { get; set; }
        public DateTime? CriadoEmServidor { get; set; }
        public DateTime? FinalizadoEmServidor { get; set; }
        public string DispositivoId { get; set; }

        public DateTime ObterCriadoMais3Horas()
        {
            return CriadoEm.AddHours(3);
        }
    }
}
