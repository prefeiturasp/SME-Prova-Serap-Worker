using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.Queries
{
    public class ObterAlunoComRespostasSemQuestoesQueryHandler : IRequestHandler<ObterAlunoComRespostasSemQuestoesQuery, IEnumerable<AlunoProvaRespostaSemPerguntaDto>>
    {
        private readonly IRepositorioQuestaoAlunoResposta repositorioQuestaoAlunoResposta;
        public ObterAlunoComRespostasSemQuestoesQueryHandler(IRepositorioQuestaoAlunoResposta repositorioQuestaoAlunoResposta)
        {
            this.repositorioQuestaoAlunoResposta = repositorioQuestaoAlunoResposta;
        }

        public Task<IEnumerable<AlunoProvaRespostaSemPerguntaDto>> Handle(ObterAlunoComRespostasSemQuestoesQuery request, CancellationToken cancellationToken)
        {
            return this.repositorioQuestaoAlunoResposta.ObterAlunoComRespostasSemQuestoes(request.Inicio, request.Fim, request.AlunoRa);
        }
    }
}
