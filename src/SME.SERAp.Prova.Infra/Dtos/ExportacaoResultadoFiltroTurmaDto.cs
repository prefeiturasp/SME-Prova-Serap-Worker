
namespace SME.SERAp.Prova.Infra
{
    public class ExportacaoResultadoFiltroTurmaDto : DtoBase
    {
        public ExportacaoResultadoFiltroTurmaDto(long processoId, long provaSerapId, long itemId, string dreEolId,
            string turmaEolId) : this()
        {
            ProcessoId = processoId;
            ProvaSerapId = provaSerapId;
            ItemId = itemId;
            DreEolId = dreEolId;
            TurmaEolId = turmaEolId;
        }

        public ExportacaoResultadoFiltroTurmaDto(long processoId, long provaSerapId, long itemId, string dreEolId,
            string turmaEolId, bool adesaoManual, bool alunosComDeficiencia) : this(processoId, provaSerapId, itemId,
            dreEolId, turmaEolId)
        {
            AdesaoManual = adesaoManual;
            AlunosComDeficiencia = alunosComDeficiencia;
        }

        public ExportacaoResultadoFiltroTurmaDto()
        {
        }

        public long ProcessoId { get; set; }
        public long ProvaSerapId { get; set; }
        public long ItemId { get; set; }
        public string DreEolId { get; set; }
        public string TurmaEolId { get; set; }
        public bool AdesaoManual { get; set; }
        public bool AlunosComDeficiencia { get; set; }
    }
}
