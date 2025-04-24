using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SME.SERAp.Prova.Aplicacao.Queries.Questao.ExisteQuestaoAlunoTaiPorId
{
    public class ExisteQuestaoAlunoTaiPorAlunoIdQueryHandler : IRequestHandler<ExisteQuestaoAlunoTaiPorAlunoIdQuery, bool>
    {
        private readonly IRepositorioQuestaoAlunoTai repositorioQuestaoAlunoTai;

        public ExisteQuestaoAlunoTaiPorAlunoIdQueryHandler(IRepositorioQuestaoAlunoTai repositorioQuestaoAlunoTai)
        {
            this.repositorioQuestaoAlunoTai = repositorioQuestaoAlunoTai ?? throw new ArgumentNullException(nameof(repositorioQuestaoAlunoTai));
        }

        public async Task<bool> Handle(ExisteQuestaoAlunoTaiPorAlunoIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioQuestaoAlunoTai.ExisteQuestaoAlunoTaiPorAlunoId(request.ProvaId, request.AlunoId);
        }
    }
}
