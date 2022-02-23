using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class QuestaoVideoPersistirCommand : IRequest<long>
    {
        public QuestaoVideoPersistirCommand(QuestaoVideo questaoVideo)
        {
            QuestaoVideo = questaoVideo;
        }

        public QuestaoVideo QuestaoVideo { get; set; }
    }
}
