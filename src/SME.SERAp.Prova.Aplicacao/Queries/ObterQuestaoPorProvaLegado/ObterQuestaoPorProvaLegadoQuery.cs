using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterQuestaoPorProvaLegadoQuery : IRequest<Questao>
    {
        public long QuestaoId { get; set; }

        public ObterQuestaoPorProvaLegadoQuery(long questaoId)
        {
            QuestaoId = questaoId;
        }
    }
}