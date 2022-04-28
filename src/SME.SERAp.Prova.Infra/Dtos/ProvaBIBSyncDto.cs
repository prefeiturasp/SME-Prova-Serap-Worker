namespace SME.SERAp.Prova.Infra
{
    public class ProvaBIBSyncDto : DtoBase
    {
        public long ProvaId { get; set; }
        public string Ano { get; set; }
        public int TotalCadernos { get; set; }
        public ProvaBIBSyncDto(long provaId, string ano, int totalCadernos)
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