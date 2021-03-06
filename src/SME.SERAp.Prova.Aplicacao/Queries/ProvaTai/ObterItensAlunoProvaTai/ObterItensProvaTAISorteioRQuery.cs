using MediatR;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterItensProvaTAISorteioRQuery : IRequest<string>
    {
        public ObterItensProvaTAISorteioRQuery(long alunoId, decimal proeficienciaAluno, IEnumerable<ItemAmostraTaiDto> itens, int quantidadeItensDaAmostra)
        {
            AlunoId = alunoId;
            ProeficienciaAluno = proeficienciaAluno;
            Itens = itens;
            QuantidadeItensDaAmostra = quantidadeItensDaAmostra;
        }

        public long AlunoId { get; set; }
        public decimal ProeficienciaAluno { get; set; }
        public IEnumerable<ItemAmostraTaiDto> Itens { get; set; }
        public int QuantidadeItensDaAmostra { get; set; }

    }
}

