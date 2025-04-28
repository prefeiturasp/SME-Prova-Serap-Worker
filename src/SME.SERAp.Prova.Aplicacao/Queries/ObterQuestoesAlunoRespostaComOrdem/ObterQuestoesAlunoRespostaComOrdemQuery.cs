using MediatR;
using SME.SERAp.Prova.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao.Queries
{
    public class ObterQuestoesAlunoRespostaComOrdemQuery : IRequest<IEnumerable<QuestaoAlunoRespostaComOrdemDto>>
    {
        public ObterQuestoesAlunoRespostaComOrdemQuery(long alunoRa, long provaId)
        {
            AlunoRa = alunoRa;
            ProvaId = provaId;
        }

        public long AlunoRa { get; set; }

        public long ProvaId { get; set; }
    }
}
