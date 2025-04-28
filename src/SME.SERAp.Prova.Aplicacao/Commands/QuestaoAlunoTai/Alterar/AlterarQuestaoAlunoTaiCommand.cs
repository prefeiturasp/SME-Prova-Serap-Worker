using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao.Commands
{
    public class AlterarQuestaoAlunoTaiCommand : IRequest<long>
    {
        public AlterarQuestaoAlunoTaiCommand(QuestaoAlunoTai questaoAlunoTai)
        {
            QuestaoAlunoTai = questaoAlunoTai;
        }

        public QuestaoAlunoTai QuestaoAlunoTai { get; set; }
    }
}
