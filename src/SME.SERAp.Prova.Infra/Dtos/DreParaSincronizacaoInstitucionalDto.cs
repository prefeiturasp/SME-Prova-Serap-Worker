namespace SME.SERAp.Prova.Infra.Dtos
{
    public class DreParaSincronizacaoInstitucionalDto
    {
        public DreParaSincronizacaoInstitucionalDto(long id, string dreCodigo)
        {
            Id = id;
            DreCodigo = dreCodigo;
        }

        public long Id { get; set; }
        public string DreCodigo { get; set; }
    }
}
