namespace SME.SERAp.Prova.Infra.Dtos.Tai
{
    public class CadernoProvaTaiTratarDto : DtoBase
    {
        public CadernoProvaTaiTratarDto(long provaId, string disciplina)
        {
            ProvaId = provaId;
            Disciplina = disciplina;
        }

        public long ProvaId { get; }
        public string Disciplina { get; }
    }
}