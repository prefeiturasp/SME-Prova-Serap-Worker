﻿using MediatR;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterItensProvaTAISorteioRQuery : IRequest<ItensProvaTAISorteioDto>
    {
        public ObterItensProvaTAISorteioRQuery(long alunoId, long alunoRa, decimal proeficienciaAluno,
            IEnumerable<ItemAmostraTaiDto> itens, int quantidadeItensDaAmostra, string componente)
        {
            AlunoId = alunoId;
            ProeficienciaAluno = proeficienciaAluno;
            Itens = itens;
            QuantidadeItensDaAmostra = quantidadeItensDaAmostra;
            Componente = componente;
            AlunoRa = alunoRa;
        }

        public long AlunoId { get; set; }
        public long AlunoRa { get; set; }
        public decimal ProeficienciaAluno { get; set; }
        public IEnumerable<ItemAmostraTaiDto> Itens { get; set; }
        public int QuantidadeItensDaAmostra { get; set; }
        public string Componente { get; set; }
    }
}

