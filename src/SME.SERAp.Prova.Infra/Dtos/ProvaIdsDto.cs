using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Infra
{
  public  class ProvaIdsDto
    {
        public ProvaIdsDto(long provaId, long provaLegadoId)
        {
            ProvaId = provaId;
            ProvaLegadoId = provaLegadoId;
        }
        public long ProvaId { get; set; }
        public long ProvaLegadoId { get; set; }
    }
}
