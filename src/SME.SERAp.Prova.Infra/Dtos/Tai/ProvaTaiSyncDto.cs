namespace SME.SERAp.Prova.Infra
{
    public class ProvaTaiSyncDto : DtoBase
    {
        public long ProvaId { get; set; }
        public long ProvaLegadoId { get; set; }
        public string Disciplina { get; set; }
        public string Ano { get; set; }
    }
}