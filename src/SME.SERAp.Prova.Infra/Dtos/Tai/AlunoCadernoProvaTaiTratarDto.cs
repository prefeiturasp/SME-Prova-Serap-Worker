using System.Collections.Generic;

namespace SME.SERAp.Prova.Infra
{
    public class AlunoCadernoProvaTaiTratarDto : DtoBase
    {
        public AlunoCadernoProvaTaiTratarDto(long provaId, long alunoId, long provaLegadoId, long alunoRa,
            string disciplina, IEnumerable<ItemAmostraTaiDto> itensAmostra, string ano, string caderno)
        {
            ProvaId = provaId;
            AlunoId = alunoId;
            ProvaLegadoId = provaLegadoId;
            AlunoRa = alunoRa;
            Disciplina = disciplina;
            ItensAmostra = itensAmostra;
            Ano = ano;
            Caderno = caderno;
        }

        public long ProvaId { get; }
        public long AlunoId { get; }
        public long ProvaLegadoId { get; }
        public long AlunoRa { get; }
        public string Disciplina { get; }
        public IEnumerable<ItemAmostraTaiDto> ItensAmostra { get; }
        public string Ano { get; }
        public string Caderno { get; }
    }
}
