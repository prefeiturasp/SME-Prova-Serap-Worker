using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmaSerapPorIdQuery : IRequest<Turma>
    {
        public ObterTurmaSerapPorIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }

}
