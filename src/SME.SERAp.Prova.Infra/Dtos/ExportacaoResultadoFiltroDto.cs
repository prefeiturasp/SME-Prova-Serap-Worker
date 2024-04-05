
namespace SME.SERAp.Prova.Infra
{
    public class ExportacaoResultadoFiltroDto : DtoBase
    {
        public ExportacaoResultadoFiltroDto(long processoId, long provaSerapId, string dreEolId, string ueEolId,
            string caminhoArquivo) : this()
        {
            ProcessoId = processoId;
            ProvaSerapId = provaSerapId;
            DreEolId = dreEolId;            
            UeEolId = ueEolId;
            CaminhoArquivo = caminhoArquivo;
        }

        public ExportacaoResultadoFiltroDto(long processoId, long provaSerapId, string dreEolId, string ueEolId,
            string caminhoArquivo, bool adesaoManual, bool alunosComDeficiencia) : this(processoId, provaSerapId,
            dreEolId, ueEolId, caminhoArquivo)
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
        public string UeEolId { get; set; }
        public string CaminhoArquivo { get; set; }
        public bool AdesaoManual { get; set; }
        public bool AlunosComDeficiencia { get; set; }
    }
}
