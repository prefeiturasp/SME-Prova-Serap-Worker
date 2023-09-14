using System.Collections.Generic;

namespace SME.SERAp.Prova.Infra.Dtos.Tai
{
    public class CadernoProvaTaiTratarDto : DtoBase
    {
        public CadernoProvaTaiTratarDto(long provaId, long provaLegadoId, string disciplina,
            List<ProvaAlunoTaiSemCadernoDto> alunosProvaTaiSemCaderno, int numeroItensAmostra,
            List<ItemAmostraTaiDto> itensAmostra, string ano)
        {
            ProvaId = provaId;
            ProvaLegadoId = provaLegadoId;
            Disciplina = disciplina;
            AlunosProvaTaiSemCaderno = alunosProvaTaiSemCaderno;
            NumeroItensAmostra = numeroItensAmostra;
            ItensAmostra = itensAmostra;
            Ano = ano;
        }

        public long ProvaId { get; }
        public long ProvaLegadoId { get; }
        public string Disciplina { get; }
        public List<ProvaAlunoTaiSemCadernoDto> AlunosProvaTaiSemCaderno { get; }
        public List<ItemAmostraTaiDto> ItensAmostra { get; }
        public int NumeroItensAmostra { get; }
        public string Ano { get; }
    }
}