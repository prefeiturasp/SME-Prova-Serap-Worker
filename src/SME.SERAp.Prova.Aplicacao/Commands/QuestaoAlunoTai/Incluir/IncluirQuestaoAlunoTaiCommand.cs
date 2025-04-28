using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao.Commands
{
    public class IncluirQuestaoAlunoTaiCommand : IRequest<long>
    {
        public IncluirQuestaoAlunoTaiCommand(QuestaoAlunoTai questaoAlunoTai)
        {
            QuestaoAlunoTai = questaoAlunoTai;
        }

        public QuestaoAlunoTai QuestaoAlunoTai { get; set; }
    }
}
