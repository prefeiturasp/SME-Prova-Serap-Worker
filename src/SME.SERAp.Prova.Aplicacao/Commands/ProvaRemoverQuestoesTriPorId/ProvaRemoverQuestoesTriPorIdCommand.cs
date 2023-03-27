using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ProvaRemoverQuestoesTriPorIdCommand : IRequest<bool>
    {
        public ProvaRemoverQuestoesTriPorIdCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}
