using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ConsultarSeExisteItemProcessoPorIdQuery : IRequest<bool>
    {
        public ConsultarSeExisteItemProcessoPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }
}
