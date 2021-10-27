using System.Collections.Generic;

namespace SME.SERAp.Prova.Infra
{
    public class ProvaBIBSyncDto
    {
        public long ProvaId { get; set; }
        public int Ano { get; set; }
        public int TotalCadernos { get; set; }
        public ProvaBIBSyncDto(long provaId, int ano, int totalCadernos)
        {
            ProvaId = provaId;
            Ano = ano;
            TotalCadernos = totalCadernos;
        }

        public ProvaBIBSyncDto()
        {
        }
    }
}