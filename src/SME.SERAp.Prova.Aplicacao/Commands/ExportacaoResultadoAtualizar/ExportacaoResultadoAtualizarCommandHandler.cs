﻿using MediatR;
using Sentry;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExportacaoResultadoAtualizarCommandHandler : IRequestHandler<ExportacaoResultadoAtualizarCommand, long>
    {
        private readonly IRepositorioExportacaoResultado repositorioExportacaoResultado;
        private readonly IRepositorioCache repositorioCache;

        public ExportacaoResultadoAtualizarCommandHandler(IRepositorioExportacaoResultado repositorioExportacaoResultado, IRepositorioCache repositorioCache)
        {
            this.repositorioExportacaoResultado = repositorioExportacaoResultado ?? throw new ArgumentNullException(nameof(repositorioExportacaoResultado));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<long> Handle(ExportacaoResultadoAtualizarCommand request, CancellationToken cancellationToken)
        {
            string chaveRedis = $"exportacao-{request.ExportacaoResultado.Id}-prova-{request.ExportacaoResultado.ProvaSerapId}-status";
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
                SentrySdk.CaptureMessage($"Atualizar exportação. Id Exportação:{exportacao.Id}, Id Prova:{exportacao.ProvaSerapId}, Erro: {ex.Message}", SentryLevel.Error);
                await repositorioExportacaoResultado.UpdateAsync(request.ExportacaoResultado);
                await repositorioCache.RemoverRedisAsync(chaveRedis);
                throw ex;
            }
        }
    }
}