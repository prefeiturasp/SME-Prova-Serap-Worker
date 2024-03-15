using MediatR;
using SME.SERAp.Prova.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao.Queries.ObterQuestaoAlunoRespostaPorProvaIdEAlunoRa
{
    public class ObterQuestaoAlunoRespostaPorProvaIdEAlunoRaQuery : IRequest<IEnumerable<AlunoQuestaoRespostasDto>>
    {
        public ObterQuestaoAlunoRespostaPorProvaIdEAlunoRaQuery(long provaLegadoId, long alunoRa)
        {
            ProvaLegadoId = provaLegadoId;
            AlunoRa = alunoRa;
        }

        public long ProvaLegadoId { get; }
        public long AlunoRa { get; }
    }
}