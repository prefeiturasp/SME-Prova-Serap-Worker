using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterQuestaoPorProvaLegadoQueryHandler : IRequestHandler<ObterQuestaoPorProvaLegadoQuery, Questao>
    {
        private readonly IRepositorioQuestao repositorioQuestao;

        public ObterQuestaoPorProvaLegadoQueryHandler(IRepositorioQuestao repositorioQuestao)
        {
            this.repositorioQuestao = repositorioQuestao ?? throw new ArgumentNullException(nameof(repositorioQuestao));
        }

        public async Task<Questao> Handle(ObterQuestaoPorProvaLegadoQuery request,
            CancellationToken cancellationToken)
            => await repositorioQuestao.ObterPorIdLegadoAsync(request.QuestaoId);
    }
}