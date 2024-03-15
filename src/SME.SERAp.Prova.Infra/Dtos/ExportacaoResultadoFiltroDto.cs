
namespace SME.SERAp.Prova.Infra
{
    public class ExportacaoResultadoFiltroDto : DtoBase
    {
        public ExportacaoResultadoFiltroDto(long processoId, long provaSerapId, long itemId, string dreEolId, string[] ueEolIds)
        {
            ProcessoId = processoId;
            ProvaSerapId = provaSerapId;
            ItemId = itemId;
            DreEolId = dreEolId;
            UeEolIds = ueEolIds;
        }

        public ExportacaoResultadoFiltroDto(long processoId, long provaSerapId, long itemId, string dreEolId,
            string[] ueEolIds, bool adesaoManual, bool alunosComDeficiencia) : this(processoId, provaSerapId, itemId,
            dreEolId, ueEolIds)
        {
            AdesaoManual = adesaoManual;
            AlunosComDeficiencia = alunosComDeficiencia;
        }

        public ExportacaoResultadoFiltroDto()
        {
        }

        public long ProcessoId { get; set; }
        public long ProvaSerapId { get; set; }
        public long ItemId { get; set; }
        public string DreEolId { get; set; }
        public string[] UeEolIds { get; set; }
        public string[] TurmaEolIds { get; set; }
        public string CaminhoArquivo { get; set; }
        public bool AdesaoManual { get; set; }
        public bool AlunosComDeficiencia { get; set; }
    }
}
