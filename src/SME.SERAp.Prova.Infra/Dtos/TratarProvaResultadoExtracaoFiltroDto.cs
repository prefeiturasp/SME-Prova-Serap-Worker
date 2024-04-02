namespace SME.SERAp.Prova.Infra
{
    public class TratarProvaResultadoExtracaoFiltroDto : DtoBase
    {
        public TratarProvaResultadoExtracaoFiltroDto(long processoId, long provaSerapId, long itemId,
            string caminhoArquivo, string dreCodigoEol, string ueCodigoEol, string turmaCodigoEol) : this()
        {
            ProcessoId = processoId;
            ProvaSerapId = provaSerapId;
            ItemId = itemId;
            CaminhoArquivo = caminhoArquivo;
            DreCodigoEol = dreCodigoEol;
            UeCodigoEol = ueCodigoEol;
            TurmaCodigoEol = turmaCodigoEol;            
        }

        public TratarProvaResultadoExtracaoFiltroDto()
        {
        }

        public long ProcessoId { get; }
        public long ProvaSerapId { get; }
        public long ItemId { get; }
        public string CaminhoArquivo { get; }
        public string DreCodigoEol { get; }
        public string UeCodigoEol { get; }
        public string TurmaCodigoEol { get; }
    }
}