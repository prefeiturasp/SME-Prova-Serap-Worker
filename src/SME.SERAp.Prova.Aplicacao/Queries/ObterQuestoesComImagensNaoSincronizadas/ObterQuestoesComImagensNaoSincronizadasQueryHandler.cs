using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterQuestoesComImagensNaoSincronizadasQueryHandler : IRequestHandler<ObterQuestoesComImagensNaoSincronizadasQuery, IEnumerable<Questao>>
    {
        private readonly IRepositorioQuestao repositorioQuestao;

        public ObterQuestoesComImagensNaoSincronizadasQueryHandler(IRepositorioQuestao repositorioQuestao)
        {
            this.repositorioQuestao = repositorioQuestao ?? throw new ArgumentNullException(nameof(repositorioQuestao));
        }
        public async Task<IEnumerable<Questao>> Handle(ObterQuestoesComImagensNaoSincronizadasQuery request, CancellationToken cancellationToken)
        {
            return await repositorioQuestao.ObterQuestoesComImagemNaoSincronizadas();
        }
    }
}
