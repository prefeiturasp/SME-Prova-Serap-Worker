using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SME.SERAp.Prova.Aplicacao.Queries.ObterAlunosResultadoProvaAdesao
{
    public class ObterAlunosResultadoProvaAdesaoQueryHandler : IRequestHandler<ObterAlunosResultadoProvaAdesaoQuery, IEnumerable<ConsolidadoProvaRespostaDto>>
    {
        private readonly IRepositorioProva repositorioProva;

        public ObterAlunosResultadoProvaAdesaoQueryHandler(IRepositorioProva repositorioProva)
        {
            this.repositorioProva = repositorioProva ?? throw new ArgumentNullException(nameof(repositorioProva));
        }

        public async Task<IEnumerable<ConsolidadoProvaRespostaDto>> Handle(ObterAlunosResultadoProvaAdesaoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioProva.ObterAlunosProvaAdesaoManualPorProvaLegadoId(request.ProvaId);
        }
    }
}