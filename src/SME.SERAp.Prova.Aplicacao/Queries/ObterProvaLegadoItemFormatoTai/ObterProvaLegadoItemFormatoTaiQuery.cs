using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;

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
