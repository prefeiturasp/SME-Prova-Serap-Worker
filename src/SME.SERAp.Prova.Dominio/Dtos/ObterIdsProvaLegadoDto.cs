using System.Collections.Generic;

namespace SME.SERAp.Prova.Dominio.Dtos
{
    public class ObterIdsProvaLegadoDto
    {
        public IEnumerable<long> Ids { get; set; }

        public ObterIdsProvaLegadoDto(IEnumerable<long> ids)
        {
            Ids = ids;
        }

        public ObterIdsProvaLegadoDto()
        {
        }
    }
}