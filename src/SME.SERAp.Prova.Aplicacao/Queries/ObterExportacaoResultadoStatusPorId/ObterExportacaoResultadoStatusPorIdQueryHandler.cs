using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterExportacaoResultadoStatusPorIdQueryHandler : IRequestHandler<ObterExportacaoResultadoStatusPorIdQuery, ExportacaoResultado>
    {
        private readonly IRepositorioExportacaoResultado repositorioExportacaoResultado;
        public ObterExportacaoResultadoStatusPorIdQueryHandler(IRepositorioExportacaoResultado repositorioExportacaoResultado)
        {
            this.repositorioExportacaoResultado = repositorioExportacaoResultado ?? throw new System.ArgumentNullException(nameof(repositorioExportacaoResultado));
        }
        public async Task<ExportacaoResultado> Handle(ObterExportacaoResultadoStatusPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioExportacaoResultado.ObterPorIdAsync(request.Id);
        }
    }
}
