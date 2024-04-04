
namespace SME.SERAp.Prova.Infra
{
    public class ExportacaoResultadoFiltroTurmaDto : DtoBase
    {
        public ExportacaoResultadoFiltroTurmaDto(long processoId, long provaSerapId, long itemId, string dreEolId,
            string turmaEolId, string caminhoArquivo) : this()
        {
            ProcessoId = processoId;
            ProvaSerapId = provaSerapId;
            ItemId = itemId;
            DreEolId = dreEolId;
            TurmaEolId = turmaEolId;
            CaminhoArquivo = caminhoArquivo;
        }

        public ExportacaoResultadoFiltroTurmaDto(long processoId, long provaSerapId, long itemId, string dreEolId,
            string turmaEolId, string caminhoArquivo, bool adesaoManual, bool alunosComDeficiencia) : this(processoId,
            provaSerapId, itemId, dreEolId, turmaEolId, caminhoArquivo)
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
        public string CaminhoArquivo { get; set; }
        public bool AdesaoManual { get; set; }
        public bool AlunosComDeficiencia { get; set; }
    }
}
