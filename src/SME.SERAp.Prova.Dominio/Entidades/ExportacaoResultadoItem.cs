using System;
using System.Linq;

namespace SME.SERAp.Prova.Dominio
{
    public class ExportacaoResultadoItem : EntidadeBase
    {
        public ExportacaoResultadoItem()
        {

        }

        public ExportacaoResultadoItem(long exportacaoResultadoId, string dreCodigoEol, string[] ueCodigoEol)
        {
            ExportacaoResultadoId = exportacaoResultadoId;
            DreCodigoEol = dreCodigoEol;
            UeCodigoEol = ueCodigoEol.Any() ? string.Join(',', ueCodigoEol.Select(a => $"'{a}'")) : String.Empty;
            CriadoEm = DateTime.Now;
        }

        public long ExportacaoResultadoId { get; set; }
        public string DreCodigoEol { get; set; }
        public string UeCodigoEol { get; set; }
        public DateTime CriadoEm { get; set; }
    }
}
