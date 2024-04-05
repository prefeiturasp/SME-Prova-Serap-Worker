using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterQuestoesAtualizadasQueryHandler : IRequestHandler<ObterQuestoesAtualizadasQuery, IEnumerable<QuestaoAtualizada>>
    {
        private readonly IRepositorioQuestao repositorioQuestao;

        public ObterQuestoesAtualizadasQueryHandler(IRepositorioQuestao repositorioQuestao)
        {
            this.repositorioQuestao = repositorioQuestao ?? throw new ArgumentNullException(nameof(repositorioQuestao));
        }

        public async Task<IEnumerable<QuestaoAtualizada>> Handle(ObterQuestoesAtualizadasQuery request, CancellationToken cancellationToken)
        {
            return await repositorioQuestao.ObterQuestoesAtualizadas(request.ProvaId, request.Pagina, request.Quantidade);
        }
    }
}
