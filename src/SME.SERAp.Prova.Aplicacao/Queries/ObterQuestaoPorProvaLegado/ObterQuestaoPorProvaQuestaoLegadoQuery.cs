using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterQuestaoPorProvaQuestaoLegadoQuery : IRequest<Questao>
    {
        public long QuestaoId { get; set; }
        public long ProvaId { get; set; }

        public ObterQuestaoPorProvaQuestaoLegadoQuery(long provaId, long questaoId)
        {
            QuestaoId = questaoId;
            ProvaId = provaId;
        }
    }
}