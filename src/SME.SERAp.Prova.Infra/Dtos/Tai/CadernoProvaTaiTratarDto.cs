using System.Collections.Generic;

namespace SME.SERAp.Prova.Infra.Dtos.Tai
{
    public class CadernoProvaTaiTratarDto : DtoBase
    {
        public CadernoProvaTaiTratarDto(long provaId, long provaLegadoId, string disciplina,
            IEnumerable<ProvaAlunoTaiSemCadernoDto> alunosProvaTaiSemCaderno,
            IEnumerable<ItemAmostraTaiDto> itensAmostra, string ano)
        {
            ProvaId = provaId;
            ProvaLegadoId = provaLegadoId;
            Disciplina = disciplina;
            AlunosProvaTaiSemCaderno = alunosProvaTaiSemCaderno;
            ItensAmostra = itensAmostra;
            Ano = ano;
        }

        public long ProvaId { get; }
        public long ProvaLegadoId { get; }
        public string Disciplina { get; }
        public IEnumerable<ProvaAlunoTaiSemCadernoDto> AlunosProvaTaiSemCaderno { get; }
        public IEnumerable<ItemAmostraTaiDto> ItensAmostra { get; }
        public string Ano { get; }
    }
}