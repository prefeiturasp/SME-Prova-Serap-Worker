using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Infra
{
    public class ConsolidarProvaFiltroDto
    {
        public ConsolidarProvaFiltroDto(long processoId, long provaId, long itemId, string dreEolId, string[] ueEolIds)
        {
            ProcessoId = processoId;
            ProvaId = provaId;
            ItemId = itemId;
            DreEolId = dreEolId;
            UeEolIds = ueEolIds;
        }

        public long ProcessoId { get; set; }
        public long ProvaId { get; set; }
        public long ItemId { get; set; }
        public string DreEolId { get; set; }
        public string[] UeEolIds { get; set; }
    }
}
