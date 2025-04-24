namespace SME.SERAp.Prova.Infra
{
    public class AlunoCadernoProvaTaiTratarDto : DtoBase
    {

        public AlunoCadernoProvaTaiTratarDto()
        {
            
        }
        public AlunoCadernoProvaTaiTratarDto(long provaId, long alunoId, long provaLegadoId, long alunoRa,
            string disciplina, string ano, string caderno)
        {
            ProvaId = provaId;
            AlunoId = alunoId;
            ProvaLegadoId = provaLegadoId;
            AlunoRa = alunoRa;
            Disciplina = disciplina;
            Ano = ano;
            Caderno = caderno;
        }

        public long ProvaId { get; }
        public long AlunoId { get; }
        public long ProvaLegadoId { get; }
        public long AlunoRa { get; }
        public string Disciplina { get; }
        public string Ano { get; }
        public string Caderno { get; }
    }


    public class AlunoCadernoProvaTaiTratarDto2
    {

        public AlunoCadernoProvaTaiTratarDto2()
        {

        }

        public long ProvaId { get; set; }
        public long AlunoId { get; set; }
        public string Caderno { get; set; }
        public long AlunoRa { get; set; }
    }
}
