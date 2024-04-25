using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterExportacaoResultadoStatusQuery : IRequest<ExportacaoResultado>
    {
        public ObterExportacaoResultadoStatusQuery(long id, long provaSerapId)
        {
            Id = id;
            ProvaSerapId = provaSerapId;
        }

        public long Id { get; }
        public long ProvaSerapId { get; }
    }
}
