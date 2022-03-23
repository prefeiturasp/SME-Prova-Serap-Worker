using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUePorIdQuery : IRequest<Ue>
    {
        public ObterUePorIdQuery(long ueId)
        {
            UeId = ueId;
        }

        public long UeId { get; set; }

    }
}
