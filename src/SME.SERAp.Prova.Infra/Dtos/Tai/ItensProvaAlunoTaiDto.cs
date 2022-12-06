using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Infra.Dtos
{
    public class ItensProvaAlunoTaiDto
    {
        public long AlunoId { get; set; }
        public long ProficienciaAluno { get; set; }
        public IEnumerable<ItemAmostraTaiDto> Itens { get; set; }
        public long QuantidadeItensDaAmostra { get; set; }

    }
}
