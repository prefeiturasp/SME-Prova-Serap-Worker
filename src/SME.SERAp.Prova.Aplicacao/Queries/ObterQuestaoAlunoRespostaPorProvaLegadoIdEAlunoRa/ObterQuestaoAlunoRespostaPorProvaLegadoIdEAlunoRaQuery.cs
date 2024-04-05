using MediatR;
using SME.SERAp.Prova.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterQuestaoAlunoRespostaPorProvaLegadoIdEAlunoRaQuery : IRequest<IEnumerable<AlunoQuestaoRespostasDto>>
    {
        public ObterQuestaoAlunoRespostaPorProvaLegadoIdEAlunoRaQuery(long provaLegadoId, long alunoRa)
        {
            ProvaLegadoId = provaLegadoId;
            AlunoRa = alunoRa;
        }

        public long ProvaLegadoId { get; }
        public long AlunoRa { get; }
    }
}