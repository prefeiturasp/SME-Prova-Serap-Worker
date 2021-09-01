using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlternativaDetalheLegadoPorIdQuery : IRequest<AlternativasProvaIdDto>
    {
        public long QuestaoId { get; private set; }
        public long AlternativaId { get; private set; }

        public ObterAlternativaDetalheLegadoPorIdQuery(long questaoId, long alternativaId)
        {
            QuestaoId = questaoId;
            AlternativaId = alternativaId;
        }
    }
}