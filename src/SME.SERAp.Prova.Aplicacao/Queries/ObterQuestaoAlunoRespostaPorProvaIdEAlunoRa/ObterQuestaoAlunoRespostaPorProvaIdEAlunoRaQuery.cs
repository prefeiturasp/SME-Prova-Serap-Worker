using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Aplicacao.Queries.ObterQuestaoAlunoRespostaPorProvaIdEAlunoRa
{
    public class ObterQuestaoAlunoRespostaPorProvaIdEAlunoRaQuery : IRequest<IEnumerable<AlunoQuestaoRespostasDto>>
    {
        public ObterQuestaoAlunoRespostaPorProvaIdEAlunoRaQuery(long provaLegadoId, long alunoRa)
        {
            ProvaLegadoId = provaLegadoId;
            AlunoRa = alunoRa;
        }

        public long ProvaLegadoId { get; set; }
        public long AlunoRa { get; set; }
    }
}