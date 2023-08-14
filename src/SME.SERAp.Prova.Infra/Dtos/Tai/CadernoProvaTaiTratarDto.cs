using System.Collections.Generic;

namespace SME.SERAp.Prova.Infra.Dtos.Tai
{
    public class CadernoProvaTaiTratarDto : DtoBase
    {
        public CadernoProvaTaiTratarDto(long provaId, long provaLegadoId, string disciplina,
            List<ProvaAlunoTaiSemCadernoDto> alunosProvaTaiSemCaderno, AmostraProvaTaiDto dadosDaAmostraTai,
            List<ItemAmostraTaiDto> itensAmostra)
        {
            ProvaId = provaId;
            ProvaLegadoId = provaLegadoId;
            Disciplina = disciplina;
            AlunosProvaTaiSemCaderno = alunosProvaTaiSemCaderno;
            DadosDaAmostraTai = dadosDaAmostraTai;
            ItensAmostra = itensAmostra;
        }

        public long ProvaId { get; }
        public long ProvaLegadoId { get; }
        public string Disciplina { get; }
        public List<ProvaAlunoTaiSemCadernoDto> AlunosProvaTaiSemCaderno { get; }
        public AmostraProvaTaiDto DadosDaAmostraTai { get; }
        public List<ItemAmostraTaiDto> ItensAmostra { get; }
    }
}