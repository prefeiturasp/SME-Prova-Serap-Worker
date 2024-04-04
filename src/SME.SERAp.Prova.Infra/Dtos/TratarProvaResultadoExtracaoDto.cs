namespace SME.SERAp.Prova.Infra
{
    public class TratarProvaResultadoExtracaoDto : DtoBase
    {
        public TratarProvaResultadoExtracaoDto()
        {
        }

        public long ProvaSerapId { get; set; }
        public long ExtracaoResultadoId { get; set; }
        public string CaminhoArquivo { get; set; }        
    }
}