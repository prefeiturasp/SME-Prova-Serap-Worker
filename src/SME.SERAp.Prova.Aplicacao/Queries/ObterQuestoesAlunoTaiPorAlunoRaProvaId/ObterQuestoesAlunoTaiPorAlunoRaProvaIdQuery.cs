using MediatR;
using SME.SERAp.Prova.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Aplicacao.Queries
{
    public class ObterQuestoesAlunoTaiPorAlunoRaProvaIdQuery : IRequest<IEnumerable<QuestaoAlunoTai>>
    {
        public ObterQuestoesAlunoTaiPorAlunoRaProvaIdQuery(long alunoRa, long provaId)
        {
            AlunoRa = alunoRa;
            ProvaId = provaId;
        }

        public long AlunoRa { get; set; }

        public long ProvaId { get; set; }
    }
}
