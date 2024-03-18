using MediatR;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlunosResultadoProvaDeficienciaQuery : IRequest<IEnumerable<ConsolidadoAlunoProvaDto>>
    {
        public ObterAlunosResultadoProvaDeficienciaQuery(long provaId, string[] turmasCodigos)
        {
            ProvaId = provaId;
            TurmasCodigos = turmasCodigos;
        }

        public long ProvaId { get; }
        public string[] TurmasCodigos { get; }
    }
}

