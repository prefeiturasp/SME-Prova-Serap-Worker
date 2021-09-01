using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ProvaRemoverAlternativasPorIdCommand : IRequest<bool>
    {
        public ProvaRemoverAlternativasPorIdCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}
