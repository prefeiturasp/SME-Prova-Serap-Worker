using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.Queries
{
    public class ObterQuestoesAlunoTaiPorAlunoRaProvaIdQueryHandler : IRequestHandler<ObterQuestoesAlunoTaiPorAlunoRaProvaIdQuery, IEnumerable<QuestaoAlunoTai>>
    {
        private readonly IRepositorioQuestaoAlunoTai repositorioQuestaoAlunoTai;
        public ObterQuestoesAlunoTaiPorAlunoRaProvaIdQueryHandler(IRepositorioQuestaoAlunoTai repositorioQuestaoAlunoTai)
        {
            this.repositorioQuestaoAlunoTai = repositorioQuestaoAlunoTai;
        }

        public Task<IEnumerable<QuestaoAlunoTai>> Handle(ObterQuestoesAlunoTaiPorAlunoRaProvaIdQuery request, CancellationToken cancellationToken)
        {
            return this.repositorioQuestaoAlunoTai.ObterQuestoesAlunoTaiPorAlunoRaProvaId(request.AlunoRa, request.ProvaId);
        }
    }
}
