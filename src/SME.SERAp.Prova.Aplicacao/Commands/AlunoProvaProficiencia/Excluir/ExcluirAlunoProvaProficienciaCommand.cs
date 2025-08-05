using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Aplicacao.Commands.AlunoProvaProficiencia.Excluir
{
    public class ExcluirAlunoProvaProficienciaCommand : IRequest<int>
    {
        public long ProvaId { get; set; }
        public long AlunoRa { get; set; }
        public ExcluirAlunoProvaProficienciaCommand(long provaId, long alunoRa)
        {
            ProvaId = provaId;
            AlunoRa = alunoRa;
        }
    }
}