using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ConsultarSeExisteItemProcessoPorIdQuery : IRequest<bool>
    {
        public ConsultarSeExisteItemProcessoPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; private set; }
    }
}
