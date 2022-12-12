using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class IncluirAtualizarQuestaoTriCommand : IRequest<bool>
    {

        public IncluirAtualizarQuestaoTriCommand(QuestaoTri questaoTri)
        {
            QuestaoTri = questaoTri;
        }

        public QuestaoTri QuestaoTri { get; set; }
    }
}
