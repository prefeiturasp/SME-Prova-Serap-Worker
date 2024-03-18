using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlunosResultadoProvaAdesaoTodosQueryHandler : IRequestHandler<ObterAlunosResultadoProvaAdesaoTodosQuery, IEnumerable<ConsolidadoAlunoProvaDto>>
    {
        private readonly IRepositorioProva repositorioProva;

        public ObterAlunosResultadoProvaAdesaoTodosQueryHandler(IRepositorioProva repositorioProva)
        {
            this.repositorioProva = repositorioProva ?? throw new ArgumentNullException(nameof(repositorioProva));
        }

        public async Task<IEnumerable<ConsolidadoAlunoProvaDto>> Handle(ObterAlunosResultadoProvaAdesaoTodosQuery request, CancellationToken cancellationToken)
        {
            return await repositorioProva.ObterAlunosProvaAdesaoTodosPorProvaLegadoIdETurmasCodigos(request.ProvaId, request.TurmasCodigos);
        }
    }
}