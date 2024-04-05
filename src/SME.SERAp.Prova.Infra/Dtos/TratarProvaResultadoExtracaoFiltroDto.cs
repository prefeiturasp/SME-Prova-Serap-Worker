namespace SME.SERAp.Prova.Infra
{
    public class TratarProvaResultadoExtracaoFiltroDto : DtoBase
    {
        public TratarProvaResultadoExtracaoFiltroDto(long processoId, long provaSerapId, string caminhoArquivo, 
            string dreCodigoEol, string ueCodigoEol) : this()
        {
            ProcessoId = processoId;
            ProvaSerapId = provaSerapId;
            CaminhoArquivo = caminhoArquivo;
            DreCodigoEol = dreCodigoEol;
            UeCodigoEol = ueCodigoEol;
        }

        public TratarProvaResultadoExtracaoFiltroDto()
        {
        }

        public long ProcessoId { get; set; }
        public long ProvaSerapId { get; set; }
        public string CaminhoArquivo { get; set; }
        public string DreCodigoEol { get; set; }
        public string UeCodigoEol { get; set; }
    }
}