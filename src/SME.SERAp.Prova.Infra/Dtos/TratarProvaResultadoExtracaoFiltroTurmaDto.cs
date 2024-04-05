namespace SME.SERAp.Prova.Infra
{
    public class TratarProvaResultadoExtracaoFiltroTurmaDto : DtoBase
    {
        public TratarProvaResultadoExtracaoFiltroTurmaDto(long processoId, long provaSerapId, string dreEolId,
            string ueEolId, string turmaEolId, long itemId, string caminhoArquivo) : this()
        {
            ProcessoId = processoId;
            ProvaSerapId = provaSerapId;
            DreEolId = dreEolId;
            UeEolId = ueEolId;
            TurmaEolId = turmaEolId;
            ItemId = itemId;
            CaminhoArquivo = caminhoArquivo;
        }

        public TratarProvaResultadoExtracaoFiltroTurmaDto()
        {
        }

        public long ProcessoId { get; set; }
        public long ProvaSerapId { get; set; }
        public string DreEolId { get; set; }
        public string UeEolId { get; set; }
        public string TurmaEolId { get; set; }
        public long ItemId { get; set; }
        public string CaminhoArquivo { get; set; }
    }
}