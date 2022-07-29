using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class RemoverQuestaoCacheCommand : IRequest<bool>
    {
        public RemoverQuestaoCacheCommand(long questaoId, long questaoLegadoId)
        {
            QuestaoId = questaoId;
            QuestaoLegadoId = questaoLegadoId;
        }

        public long QuestaoId { get; set; }

        public long QuestaoLegadoId { get; set; }
    }
}
