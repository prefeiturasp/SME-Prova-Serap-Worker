using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Aplicacao
{
    public class VerificaAlunoProvaProficienciaExisteQuery : IRequest<bool>
    {
        public VerificaAlunoProvaProficienciaExisteQuery(long alunoId, long provaId)
        {
            AlunoId = alunoId;
            ProvaId = provaId;
        }

        public long AlunoId { get; set; }
        public long ProvaId { get; set; }
    }
}
