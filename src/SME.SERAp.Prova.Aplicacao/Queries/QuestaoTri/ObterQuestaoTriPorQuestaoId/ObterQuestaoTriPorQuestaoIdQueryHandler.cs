using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterQuestaoTriPorQuestaoIdQueryHandler : IRequestHandler<ObterQuestaoTriPorQuestaoIdQuery, QuestaoTri>
    {

        private readonly IRepositorioQuestaoTri repositorioQuestaoTri;

        public ObterQuestaoTriPorQuestaoIdQueryHandler(IRepositorioQuestaoTri repositorioQuestaoTri)
        {
            this.repositorioQuestaoTri = repositorioQuestaoTri ?? throw new ArgumentException(nameof(repositorioQuestaoTri));
        }

        public async Task<QuestaoTri> Handle(ObterQuestaoTriPorQuestaoIdQuery request,
            CancellationToken cancellationToken)
            => await repositorioQuestaoTri.ObterPorQuestaoIdAsync(request.QuestaoId);
    }
}
