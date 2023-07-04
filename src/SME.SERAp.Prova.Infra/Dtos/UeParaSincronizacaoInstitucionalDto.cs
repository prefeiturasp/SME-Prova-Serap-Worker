namespace SME.SERAp.Prova.Infra
{
    public class UeParaSincronizacaoInstitucionalDto : DtoBase
    {
        public UeParaSincronizacaoInstitucionalDto(long id, string ueCodigo, long dreId, string dreCodigo)
        {
            Id = id;
            UeCodigo = ueCodigo;
            DreId = dreId;
            DreCodigo = dreCodigo;
        }

        public long Id { get; private set; }
        public string UeCodigo { get; private set; }
        public long DreId { get; private set; }
        public string DreCodigo { get; private set; }
    }
}
