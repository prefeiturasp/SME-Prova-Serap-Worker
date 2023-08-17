using System.Collections.Generic;

namespace SME.SERAp.Prova.Infra
{
    public class AlunoCadernoProvaTaiTratarDto : DtoBase
    {
        public AlunoCadernoProvaTaiTratarDto(long provaId, long alunoId, long provaLegadoId, long alunoRa,
            string disciplina, List<ItemAmostraTaiDto> itensAmostra, int numeroItensAmostra, string ano)
        {
            ProvaId = provaId;
            AlunoId = alunoId;
            ProvaLegadoId = provaLegadoId;
            AlunoRa = alunoRa;
            Disciplina = disciplina;
            ItensAmostra = itensAmostra;
            NumeroItensAmostra = numeroItensAmostra;
            Ano = ano;
        }

        public long ProvaId { get; }
        public long AlunoId { get; }
        public long ProvaLegadoId { get; }
        public long AlunoRa { get; }
        public string Disciplina { get; }
        public List<ItemAmostraTaiDto> ItensAmostra { get; }
        public int NumeroItensAmostra { get; }
        public string Ano { get; }
    }
}
