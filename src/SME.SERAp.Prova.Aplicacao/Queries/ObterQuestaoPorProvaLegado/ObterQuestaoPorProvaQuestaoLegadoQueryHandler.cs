using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterQuestaoPorProvaQuestaoLegadoQueryHandler : IRequestHandler<ObterQuestaoPorProvaQuestaoLegadoQuery, Questao>
    {
        private readonly IRepositorioQuestao repositorioQuestao;

        public ObterQuestaoPorProvaQuestaoLegadoQueryHandler(IRepositorioQuestao repositorioQuestao)
        {
            this.repositorioQuestao = repositorioQuestao ?? throw new ArgumentNullException(nameof(repositorioQuestao));
        }

        public async Task<Questao> Handle(ObterQuestaoPorProvaQuestaoLegadoQuery request,
            CancellationToken cancellationToken)
            => await repositorioQuestao.ObterPorIdEProvaIdLegadoAsync(request.QuestaoId, request.ProvaId);
    }
}