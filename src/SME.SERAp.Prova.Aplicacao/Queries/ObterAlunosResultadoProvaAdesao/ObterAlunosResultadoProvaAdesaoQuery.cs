using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Aplicacao.Queries.ObterAlunosResultadoProvaAdesao
{
    public class ObterAlunosResultadoProvaAdesaoQuery : IRequest<IEnumerable<ConsolidadoProvaRespostaDto>>
    {
        public ObterAlunosResultadoProvaAdesaoQuery(long provaId)
        {
            ProvaId = provaId;
        }

        public long ProvaId { get; set; }
    }
}
