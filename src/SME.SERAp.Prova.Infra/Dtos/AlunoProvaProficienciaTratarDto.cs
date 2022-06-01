namespace SME.SERAp.Prova.Infra
{
    public class AlunoProvaProficienciaTratarDto : DtoBase
    {
        public AlunoProvaProficienciaTratarDto(long? provaId, long? alunoId)
        {
            ProvaId = provaId;
            AlunoId = alunoId;
        }

        public long? ProvaId { get; set; }
        public long? AlunoId { get; set; }
    }
}
