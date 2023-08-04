namespace SME.SERAp.Prova.Infra
{
    public class TurmaParaSincronizacaoInstitucionalDto : DtoBase
    {
        public TurmaParaSincronizacaoInstitucionalDto(long id, int anoLetivo, string codigo, int modalidadeCodigo,
            int semestre, long ueId, string ueCodigo)
        {
            Id = id;
            AnoLetivo = anoLetivo;
            Codigo = codigo;
            ModalidadeCodigo = modalidadeCodigo;
            Semestre = semestre;
            UeId = ueId;
            UeCodigo = ueCodigo;
        }

        public long Id { get; }
        public int AnoLetivo { get; }
        public string Codigo { get; }
        public int ModalidadeCodigo { get; }
        public int Semestre { get; }
        public long UeId { get; }
        public string UeCodigo { get; }
    }
}