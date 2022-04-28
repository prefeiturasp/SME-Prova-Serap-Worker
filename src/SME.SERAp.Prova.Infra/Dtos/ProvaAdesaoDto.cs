namespace SME.SERAp.Prova.Infra
{
    public class ProvaAdesaoDto : DtoBase
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
