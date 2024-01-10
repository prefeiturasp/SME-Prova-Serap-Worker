namespace SME.SERAp.Prova.Infra.Dtos.Tai
{
    public class CadernoProvaTaiTratarDto : DtoBase
    {
        public CadernoProvaTaiTratarDto(long provaId, long provaLegadoId, string disciplina, string ano, long alunoId,
            long alunoRa)
        {
            ProvaId = provaId;
            ProvaLegadoId = provaLegadoId;
            Disciplina = disciplina;
            Ano = ano;
            AlunoId = alunoId;
            AlunoRa = alunoRa;
        }

        public long ProvaId { get; }
        public long ProvaLegadoId { get; }
        public string Disciplina { get; }
        public string Ano { get; }
        public long AlunoId { get; }
        public long AlunoRa { get; }
    }
}