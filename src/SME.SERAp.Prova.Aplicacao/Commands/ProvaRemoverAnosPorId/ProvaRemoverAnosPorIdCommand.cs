using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ProvaRemoverAnosPorIdCommand : IRequest<bool>
    {
        public ProvaRemoverAnosPorIdCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}
