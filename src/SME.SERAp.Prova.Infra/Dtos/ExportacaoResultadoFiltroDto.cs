
namespace SME.SERAp.Prova.Infra
{
    public class ExportacaoResultadoFiltroDto : DtoBase
    {
        public ExportacaoResultadoFiltroDto(long processoId, long provaId, long itemId, string dreEolId, string[] ueEolIds)
        {
            ProcessoId = processoId;
            ProvaId = provaId;
            ItemId = itemId;
            DreEolId = dreEolId;
            UeEolIds = ueEolIds;
        }

        public ExportacaoResultadoFiltroDto(long processoId, long provaId, string caminhoArquivo)
        {
            ProcessoId = processoId;
            ProvaId = provaId;
            CaminhoArquivo = caminhoArquivo;

        }

        public ExportacaoResultadoFiltroDto(long processoId, long provaId, long itemId, string dreEolId, string[] ueEolIds, bool adesaoManual, bool alunosComDeficiencia)
        {
            ProcessoId = processoId;
            ProvaId = provaId;
            ItemId = itemId;
            DreEolId = dreEolId;
            UeEolIds = ueEolIds;
            AdesaoManual = adesaoManual;
            AlunosComDeficiencia = alunosComDeficiencia;

        }


        public ExportacaoResultadoFiltroDto()
        {

        }

        public long ProcessoId { get; set; }
        public long ProvaId { get; set; }
        public long ItemId { get; set; }
        public string DreEolId { get; set; }
        public string[] UeEolIds { get; set; }
        public string[] TurmaEolIds { get; set; }
        public string CaminhoArquivo { get; set; }
        public bool AdesaoManual { get; set; }
        public bool AlunosComDeficiencia { get; set; }

    }
}
