using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ConsolidarProvaRespostaPorProvaSerapIdCommand : IRequest<bool>
    {
        public ConsolidarProvaRespostaPorProvaSerapIdCommand(long provaId)
        {
            ProvaId = provaId;
        }

        public long ProvaId { get; set; }
    }
}
