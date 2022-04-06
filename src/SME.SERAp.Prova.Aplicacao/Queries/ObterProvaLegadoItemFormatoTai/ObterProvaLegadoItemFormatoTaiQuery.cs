using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterProvaLegadoItemFormatoTaiQuery : IRequest<ProvaFormatoTaiItem?>
    {
        public ObterProvaLegadoItemFormatoTaiQuery(long provaLegadoId)
        {
            ProvaLegadoId = provaLegadoId;
        }

        public long ProvaLegadoId { get; set; }
    }
}
