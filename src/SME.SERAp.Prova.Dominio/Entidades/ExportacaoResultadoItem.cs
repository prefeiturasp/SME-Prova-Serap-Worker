using System;

namespace SME.SERAp.Prova.Dominio
{
    public class ExportacaoResultadoItem : EntidadeBase
    {
        public ExportacaoResultadoItem()
        {
        }

        public ExportacaoResultadoItem(long exportacaoResultadoId, string dreCodigoEol, string ueCodigoEol,
            string turmaCodigoEol = null) : this()
        {
            ExportacaoResultadoId = exportacaoResultadoId;
            DreCodigoEol = dreCodigoEol;
            UeCodigoEol = ueCodigoEol;
            TurmaCodigoEol = turmaCodigoEol;
            CriadoEm = DateTime.Now;
        }

        public long ExportacaoResultadoId { get; set; }
        public string DreCodigoEol { get; set; }
        public string UeCodigoEol { get; set; }
        public string TurmaCodigoEol { get; set; }
        public DateTime CriadoEm { get; set; }
    }
}
