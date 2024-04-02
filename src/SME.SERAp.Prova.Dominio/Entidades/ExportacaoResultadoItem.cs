using System;
using System.Linq;

namespace SME.SERAp.Prova.Dominio
{
    public class ExportacaoResultadoItem : EntidadeBase
    {
        public ExportacaoResultadoItem()
        {
        }

        public ExportacaoResultadoItem(long exportacaoResultadoId, string dreCodigoEol, string[] ueCodigoEol, string[] turmaCodigoEol) : this()
        {
            ExportacaoResultadoId = exportacaoResultadoId;
            DreCodigoEol = dreCodigoEol;
            UeCodigoEol = ueCodigoEol.Any() ? string.Join(',', ueCodigoEol.Select(a => $"'{a}'")) : string.Empty;
            TurmaCodigoEol = turmaCodigoEol.Any() ? string.Join(',', turmaCodigoEol.Select(a => $"'{a}'")) : string.Empty;
            CriadoEm = DateTime.Now;
        }

        public long ExportacaoResultadoId { get; set; }
        public string DreCodigoEol { get; set; }
        public string UeCodigoEol { get; set; }
        public string TurmaCodigoEol { get; set; }
        public DateTime CriadoEm { get; set; }
    }
}
