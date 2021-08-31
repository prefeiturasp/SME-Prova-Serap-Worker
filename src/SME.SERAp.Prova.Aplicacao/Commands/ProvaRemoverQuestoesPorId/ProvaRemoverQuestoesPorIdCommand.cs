using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ProvaRemoverQuestoesPorIdCommand : IRequest<bool>
    {
        public ProvaRemoverQuestoesPorIdCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}
