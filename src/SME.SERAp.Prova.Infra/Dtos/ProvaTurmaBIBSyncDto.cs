namespace SME.SERAp.Prova.Infra
{
    public class ProvaTurmaBIBSyncDto : DtoBase
    {
        public long ProvaId { get; set; }
        public long TurmaId { get; set; }
        public int TotalCadernos { get; set; }
        public ProvaTurmaBIBSyncDto(long provaId, int totalCadernos, long turmaId)
        {
            ProvaId = provaId;
            TotalCadernos = totalCadernos;
            TurmaId = turmaId;
        }

        public ProvaTurmaBIBSyncDto()
        {
        }
    }
}