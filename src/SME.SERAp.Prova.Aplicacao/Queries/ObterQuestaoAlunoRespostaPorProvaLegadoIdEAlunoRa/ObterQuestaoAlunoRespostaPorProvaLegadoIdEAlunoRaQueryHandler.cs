using MediatR;
using SME.SERAp.Prova.Dados;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using SME.SERAp.Prova.Infra.Dtos;

namespace SME.SERAp.Prova.Aplicacao.Queries.ObterQuestaoAlunoRespostaPorProvaIdEAlunoRa
{
    public class ObterQuestaoAlunoRespostaPorProvaLegadoIdEAlunoRaQueryHandler : IRequestHandler<ObterQuestaoAlunoRespostaPorProvaLegadoIdEAlunoRaQuery, IEnumerable<AlunoQuestaoRespostasDto>>
    {
        private readonly IRepositorioResultadoProvaConsolidado repositorioResultadoProvaConsolidado;

        public ObterQuestaoAlunoRespostaPorProvaLegadoIdEAlunoRaQueryHandler(IRepositorioResultadoProvaConsolidado repositorioResultadoProvaConsolidado)
        {
            this.repositorioResultadoProvaConsolidado = repositorioResultadoProvaConsolidado ?? throw new System.ArgumentNullException(nameof(repositorioResultadoProvaConsolidado));
        }

        public async Task<IEnumerable<AlunoQuestaoRespostasDto>> Handle(ObterQuestaoAlunoRespostaPorProvaLegadoIdEAlunoRaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioResultadoProvaConsolidado.ObterQuestaoAlunoRespostaPorProvaLegadoIdEAlunoRA(request.ProvaLegadoId, request.AlunoRa, request.PossuiBib);
        }
    }
}