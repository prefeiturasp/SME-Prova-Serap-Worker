using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExcluirProvaAlunoCommand : IRequest<int>
    {

        public ExcluirProvaAlunoCommand(long provaId, long alunoRa)
        {
            ProvaId = provaId;
            AlunoRa = alunoRa;
        }

       public long ProvaId { get; set; }
       public long AlunoRa { get; set; }
    }
}


