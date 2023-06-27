using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ConsolidarProvaRespostaPorFiltroCommand : IRequest<bool>
    {
        public ConsolidarProvaRespostaPorFiltroCommand(long provaId, string dreEolId, string[] ueEolIds, string[] turmaEolIds)
        {
            ProvaId = provaId;
            DreEolId = dreEolId;
            UeEolIds = ueEolIds;
            TurmaEolIds = turmaEolIds;
        }

        public long ProvaId { get; set; }
        public string DreEolId { get; set; }
        public string[] UeEolIds { get; set; }
        public string[] TurmaEolIds { get; set; }
    }
}
