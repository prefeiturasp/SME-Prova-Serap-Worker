using SME.SERAp.Prova.Dominio;
using System;

namespace SME.SERAp.Prova.Infra
{
    public class ProvaAlunoDto : DtoBase
    {
        public long ProvaAlunoId { get; set; }
        public long ProvaId { get; set; }
        public DateTime ProvaIniciadaEm { get; set; }
        public int Modalidade { get; set; }
        public DateTime InicioProva { get; set; }
        public DateTime FimProva { get; set; }
        public long TempoExecucao { get; set; }
        public int Status { get; set; }

        public bool PodeFinalizarProva(int tempoExtra)
        {
            ProvaStatus provaStatus = (ProvaStatus)Status;
            if (provaStatus == ProvaStatus.Finalizado || provaStatus == ProvaStatus.FinalizadoAutomaticamente)
                return false;

            if (TempoExecucao > 0)
            {
                int tempoTotal = ObterTempoTotal();
                return tempoTotal > (TempoExecucao + tempoExtra);
            }
            return DateTime.Now > FimProva;
        }

        public int ObterTempoTotal()
        {
            if (TempoExecucao > 0)
            {
                TimeSpan tempoTotal = DateTime.Now - ProvaIniciadaEm;
                return (int)tempoTotal.TotalSeconds;
            }
            return 0;
        }

    }
}