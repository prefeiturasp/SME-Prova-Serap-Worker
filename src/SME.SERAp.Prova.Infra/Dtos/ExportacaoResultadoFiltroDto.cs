
namespace SME.SERAp.Prova.Infra
{
    public class ExportacaoResultadoFiltroDto : DtoBase
    {
        public ExportacaoResultadoFiltroDto(long processoId, long provaSerapId, string dreEolId) : this()
        {
            ProcessoId = processoId;
            ProvaSerapId = provaSerapId;
            DreEolId = dreEolId;
        }

        public ExportacaoResultadoFiltroDto(long processoId, long provaSerapId, string dreEolId,
            bool adesaoManual, bool alunosComDeficiencia) : this(processoId, provaSerapId,
            dreEolId)
        {
            AdesaoManual = adesaoManual;
            AlunosComDeficiencia = alunosComDeficiencia;
        }

        public ExportacaoResultadoFiltroDto()
        {
        }

        public long ProcessoId { get; set; }
        public long ProvaSerapId { get; set; }
        public string DreEolId { get; set; }
        public bool AdesaoManual { get; set; }
        public bool AlunosComDeficiencia { get; set; }
    }
}
