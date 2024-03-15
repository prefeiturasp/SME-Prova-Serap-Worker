using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SME.SERAp.Prova.Infra.Dtos;

namespace SME.SERAp.Prova.Aplicacao.Queries.ObterQuestaoAlunoRespostaPorProvaIdEAlunoRa
{
    public class ObterQuestaoAlunoRespostaPorProvaIdEAlunoRaQueryHandler : IRequestHandler<ObterQuestaoAlunoRespostaPorProvaIdEAlunoRaQuery, IEnumerable<AlunoQuestaoRespostasDto>>
    {
        private readonly IRepositorioResultadoProvaConsolidado repositorioResultadoProvaConsolidado;

        public ObterQuestaoAlunoRespostaPorProvaIdEAlunoRaQueryHandler(IRepositorioResultadoProvaConsolidado repositorioResultadoProvaConsolidado)
        {
            this.repositorioResultadoProvaConsolidado = repositorioResultadoProvaConsolidado ?? throw new System.ArgumentNullException(nameof(repositorioResultadoProvaConsolidado));
        }
        public async Task<IEnumerable<AlunoQuestaoRespostasDto>> Handle(ObterQuestaoAlunoRespostaPorProvaIdEAlunoRaQuery request, CancellationToken cancellationToken)
        {
            return await   repositorioResultadoProvaConsolidado.ObterQuestaoAlunoRespostaPorProvaLegadoIdEAlunoRA(request.ProvaLegadoId, request.AlunoRa);
        }
    }
}