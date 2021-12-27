using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Infra
{
    public class ConsolidarProvaFiltroDto
    {
        public ConsolidarProvaFiltroDto(long processoId, long provaId, string dreEolId, string[] ueEolIds, bool finalizarProcesso = false)
        {
            ProcessoId = processoId;
            ProvaId = provaId;
            DreEolId = dreEolId;
            UeEolIds = ueEolIds;
            FinalizarProcesso = finalizarProcesso;
        }

        public long ProcessoId { get; set; }
        public long ProvaId { get; set; }
        public string DreEolId { get; set; }
        public string[] UeEolIds { get; set; }

        public bool FinalizarProcesso { get; private set; }
    }
}
