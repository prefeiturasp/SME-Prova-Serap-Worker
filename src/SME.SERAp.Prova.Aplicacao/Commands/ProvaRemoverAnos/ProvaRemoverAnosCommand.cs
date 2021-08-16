using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ProvaRemoverAnosCommand : IRequest<bool>
    {
        public ProvaRemoverAnosCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}
