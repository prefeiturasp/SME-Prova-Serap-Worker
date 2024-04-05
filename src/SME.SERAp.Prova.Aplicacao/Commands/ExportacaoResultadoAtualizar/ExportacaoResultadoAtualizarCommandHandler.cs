using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Dominio.Enums;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExportacaoResultadoAtualizarCommandHandler : IRequestHandler<ExportacaoResultadoAtualizarCommand, long>
    {
        private readonly IRepositorioExportacaoResultado repositorioExportacaoResultado;
        private readonly IRepositorioCache repositorioCache;
        private readonly IServicoLog servicoLog;

        public ExportacaoResultadoAtualizarCommandHandler(IRepositorioExportacaoResultado repositorioExportacaoResultado, IRepositorioCache repositorioCache, IServicoLog servicoLog)
        {
            this.repositorioExportacaoResultado = repositorioExportacaoResultado ?? throw new ArgumentNullException(nameof(repositorioExportacaoResultado));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
            this.servicoLog = servicoLog ?? throw new ArgumentException(nameof(servicoLog));
        }

        public async Task<long> Handle(ExportacaoResultadoAtualizarCommand request, CancellationToken cancellationToken)
        {
            var chaveRedis = $"exportacao-{request.ExportacaoResultado.Id}-prova-{request.ExportacaoResultado.ProvaSerapId}-status";
            try
            {
                var exportacao = await repositorioExportacaoResultado.ObterPorIdAsync(request.ExportacaoResultado.Id);
                exportacao.AtualizarStatus(request.ExportacaoResultado.Status);
                var result = await repositorioExportacaoResultado.UpdateAsync(exportacao);
                await repositorioCache.RemoverRedisAsync(chaveRedis);
                return result;
            }
            catch(Exception ex)
            {
                var exportacao = request.ExportacaoResultado;
                exportacao.NomeArquivo = "";
                exportacao.AtualizarStatus(ExportacaoResultadoStatus.Erro);
                
                servicoLog.Registrar(LogNivel.Critico, $"Atualizar exportação.Id Exportação:{ exportacao.Id}, Id Prova:{ exportacao.ProvaSerapId}", $" Erro: { ex.Message}", ex.StackTrace);

                await repositorioExportacaoResultado.UpdateAsync(request.ExportacaoResultado);
                await repositorioCache.RemoverRedisAsync(chaveRedis);

                throw ex;
            }
        }
    }
}