using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterExportacaoResultadoStatusQueryHandler : IRequestHandler<ObterExportacaoResultadoStatusQuery, ExportacaoResultado>
    {
        private readonly IRepositorioExportacaoResultado repositorioExportacaoResultado;
        private readonly IRepositorioCache repositorioCache;
        public ObterExportacaoResultadoStatusQueryHandler(IRepositorioExportacaoResultado repositorioExportacaoResultado, IRepositorioCache repositorioCache)
        {
            this.repositorioExportacaoResultado = repositorioExportacaoResultado ?? throw new ArgumentNullException(nameof(repositorioExportacaoResultado));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }
        public async Task<ExportacaoResultado> Handle(ObterExportacaoResultadoStatusQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var exportacao = new ExportacaoResultado { Id = request.Id, ProvaSerapId = request.ProvaSerapId };
                string chaveRedis = $"exportacao-{exportacao.Id}-prova-{exportacao.ProvaSerapId}-status";
                exportacao.Status = (ExportacaoResultadoStatus)await repositorioCache.ObterRedisAsync(chaveRedis, async () => await repositorioExportacaoResultado.ObterStatusPorIdAsync(exportacao.Id));
                return exportacao;
            }
            catch(Exception ex)
            {
                throw new ArgumentException($"Obter status do processo de exportação. Exportação Id:{request.Id}, ProvaSerapId:{request.ProvaSerapId}, Erro:{ex.Message}");
            }            
        }
    }
}
