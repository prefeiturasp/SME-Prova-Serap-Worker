using MediatR;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Aplicacao.Queries.ObterAlunosResultadoProvaDeficiencia
{
    public class ObterAlunosResultadoProvaDeficienciaQuery : IRequest<IEnumerable<ConsolidadoProvaRespostaDto>>
    {

        public ObterAlunosResultadoProvaDeficienciaQuery(long provaId)
        {
            ProvaId = provaId;
        }

        public long ProvaId { get; set; }
    }
}

