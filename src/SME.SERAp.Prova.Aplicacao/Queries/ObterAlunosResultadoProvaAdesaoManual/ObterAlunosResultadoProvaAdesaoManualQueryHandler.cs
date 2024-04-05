using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlunosResultadoProvaAdesaoManualQueryHandler : IRequestHandler<ObterAlunosResultadoProvaAdesaoManualQuery, IEnumerable<ConsolidadoAlunoProvaDto>>
    {
        private readonly IRepositorioProva repositorioProva;

        public ObterAlunosResultadoProvaAdesaoManualQueryHandler(IRepositorioProva repositorioProva)
        {
            this.repositorioProva = repositorioProva ?? throw new ArgumentNullException(nameof(repositorioProva));
        }

        public async Task<IEnumerable<ConsolidadoAlunoProvaDto>> Handle(ObterAlunosResultadoProvaAdesaoManualQuery request, CancellationToken cancellationToken)
        {
            return await repositorioProva.ObterAlunosProvaAdesaoManualPorProvaLegadoIdETurmasCodigos(request.ProvaId, request.TurmasCodigos);
        }
    }
}