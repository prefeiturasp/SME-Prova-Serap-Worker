using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ConsolidarProvaRespostaPorFiltroCommand : IRequest<bool>
    {
        public ConsolidarProvaRespostaPorFiltroCommand(long provaId, string dreEolId, string[] ueEolIds)
        {
            ProvaId = provaId;
            DreEolId = dreEolId;
            UeEolIds = ueEolIds;
        }

        public long ProvaId { get; set; }
        public string DreEolId { get; set; }
        public string[] UeEolIds { get; set; }
    }
}
