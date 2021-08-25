using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterDetalheAlternativarLegadoProvaPorProvaIdQuery : IRequest<AlternativasProvaIdDto>
    {
        public long ProvaId { get; private set; }
        public long QuestaoId { get; private set; }
        public long AlternativaId { get; private set; }

        public ObterDetalheAlternativarLegadoProvaPorProvaIdQuery(long provaId, long questaoId, long alternativaId)
        {
            ProvaId = provaId;
            QuestaoId = questaoId;
            AlternativaId = alternativaId;
        }
    }
}