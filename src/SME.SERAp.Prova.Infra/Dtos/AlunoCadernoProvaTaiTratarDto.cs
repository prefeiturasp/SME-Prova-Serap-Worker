
namespace SME.SERAp.Prova.Infra
{
    public class AlunoCadernoProvaTaiTratarDto : DtoBase
    {
        public AlunoCadernoProvaTaiTratarDto(long provaId, long alunoId, long provaLegadoId)
        {
            ProvaId = provaId;
            AlunoId = alunoId;
            ProvaLegadoId = provaLegadoId;
        }

        public long ProvaId { get; set; }
        public long AlunoId { get; set; }
        public long ProvaLegadoId { get; set; }

    }    
}
