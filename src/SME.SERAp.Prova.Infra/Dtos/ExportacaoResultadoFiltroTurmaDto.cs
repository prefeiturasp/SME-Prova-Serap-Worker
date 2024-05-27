
namespace SME.SERAp.Prova.Infra
{
    public class ExportacaoResultadoFiltroTurmaDto : DtoBase
    {
        public ExportacaoResultadoFiltroTurmaDto(long processoId, long provaSerapId, long itemId, string[] turmasEolIds,
            string caminhoArquivo) : this()
        {
            ProcessoId = processoId;
            ProvaSerapId = provaSerapId;
            ItemId = itemId;
            TurmasEolIds = turmasEolIds;
            CaminhoArquivo = caminhoArquivo;
        }

        public ExportacaoResultadoFiltroTurmaDto(long processoId, long provaSerapId, long itemId,
            string[] turmasEolIds, string caminhoArquivo, bool adesaoManual, bool alunosComDeficiencia) : this(processoId,
            provaSerapId, itemId, turmasEolIds, caminhoArquivo)
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
        public string[] TurmasEolIds { get; set; }
        public string CaminhoArquivo { get; set; }
        public bool AdesaoManual { get; set; }
        public bool AlunosComDeficiencia { get; set; }
    }
}
