
namespace SME.SERAp.Prova.Infra
{
    public class AlunoCadernoProvaTaiTratarDto : DtoBase
    {
        public AlunoCadernoProvaTaiTratarDto(long provaId, long alunoId)
        {
            ProvaId = provaId;
            AlunoId = alunoId;
        }

        public long ProvaId { get; set; }
        public long AlunoId { get; set; }

    }    
}
