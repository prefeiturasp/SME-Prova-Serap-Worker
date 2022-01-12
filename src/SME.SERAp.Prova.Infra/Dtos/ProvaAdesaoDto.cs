using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Infra
{
    public class ProvaAdesaoDto
    {
        public ProvaAdesaoDto(long provaId, long provaLegadoId, bool aderirTodos)
        {
            ProvaId = provaId;
            ProvaLegadoId = provaLegadoId;
            AderirTodos = aderirTodos;
        }

        public long ProvaId { get; set; }
        public long ProvaLegadoId { get; set; }
        public bool AderirTodos { get; set; }

    }
}
