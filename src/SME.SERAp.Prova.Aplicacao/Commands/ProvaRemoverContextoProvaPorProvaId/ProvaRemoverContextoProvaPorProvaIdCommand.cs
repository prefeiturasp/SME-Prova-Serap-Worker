using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ProvaRemoverContextoProvaPorProvaIdCommand : IRequest<bool>
    {
        public ProvaRemoverContextoProvaPorProvaIdCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}
