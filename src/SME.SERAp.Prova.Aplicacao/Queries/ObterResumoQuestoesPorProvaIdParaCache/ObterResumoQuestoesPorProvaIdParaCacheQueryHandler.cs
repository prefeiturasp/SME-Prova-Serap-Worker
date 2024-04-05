using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterResumoQuestoesPorProvaIdParaCacheQueryHandler : IRequestHandler<ObterResumoQuestoesPorProvaIdParaCacheQuery, IEnumerable<ResumoQuestaoProvaDto>>
    {
        private readonly IRepositorioQuestao repositorioQuestao;

        public ObterResumoQuestoesPorProvaIdParaCacheQueryHandler(IRepositorioQuestao repositorioQuestao)
        {
            this.repositorioQuestao = repositorioQuestao ?? throw new ArgumentNullException(nameof(repositorioQuestao));
        }

        public async Task<IEnumerable<ResumoQuestaoProvaDto>> Handle(ObterResumoQuestoesPorProvaIdParaCacheQuery request, CancellationToken cancellationToken)
        {
            return await repositorioQuestao.ObterResumoQuestoesPorProvaIdParaCacheAsync(request.ProvaId);
        }
    }
}