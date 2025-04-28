using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.Queries
{
    public class ObterQuestoesAlunoRespostaComOrdemQueryHandler : IRequestHandler<ObterQuestoesAlunoRespostaComOrdemQuery, IEnumerable<QuestaoAlunoRespostaComOrdemDto>>
    {
        private readonly IRepositorioQuestaoAlunoResposta repositorioQuestaoAlunoResposta;
        public ObterQuestoesAlunoRespostaComOrdemQueryHandler(IRepositorioQuestaoAlunoResposta repositorioQuestaoAlunoResposta)
        {
            this.repositorioQuestaoAlunoResposta = repositorioQuestaoAlunoResposta;
        }
        public async Task<IEnumerable<QuestaoAlunoRespostaComOrdemDto>> Handle(ObterQuestoesAlunoRespostaComOrdemQuery request, CancellationToken cancellationToken)
        {
            return await repositorioQuestaoAlunoResposta.ObterQuestoesAlunoRespostaComOrdem(request.AlunoRa, request.ProvaId);
        }
    }
}
