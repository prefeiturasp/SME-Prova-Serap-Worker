using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterDetalheAlternativarLegadoProvaPorProvaIdQuery : IRequest<AlternativasProvaIdDto>
    {
        public long QuestaoId { get; private set; }
        public long AlternativaId { get; private set; }

        public ObterDetalheAlternativarLegadoProvaPorProvaIdQuery(long questaoId, long alternativaId)
        {
            QuestaoId = questaoId;
            AlternativaId = alternativaId;
        }
    }
}