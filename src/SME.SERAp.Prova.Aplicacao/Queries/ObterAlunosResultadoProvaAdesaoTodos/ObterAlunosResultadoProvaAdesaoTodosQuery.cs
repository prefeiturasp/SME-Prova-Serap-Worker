using System.Collections.Generic;
using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlunosResultadoProvaAdesaoTodosQuery : IRequest<IEnumerable<ConsolidadoAlunoProvaDto>>
    {
        public ObterAlunosResultadoProvaAdesaoTodosQuery(long provaId, string[] turmasCodigos)
        {
            ProvaId = provaId;
            TurmasCodigos = turmasCodigos;
        }

        public long ProvaId { get; }
        public string[] TurmasCodigos { get; }        
    }
}