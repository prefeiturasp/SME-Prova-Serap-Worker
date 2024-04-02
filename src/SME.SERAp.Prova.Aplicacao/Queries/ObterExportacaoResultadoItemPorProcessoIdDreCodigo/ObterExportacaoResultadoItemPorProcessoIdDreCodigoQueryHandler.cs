using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterExportacaoResultadoItemPorProcessoIdDreCodigoQueryHandler : IRequestHandler<ObterExportacaoResultadoItemPorProcessoIdDreCodigoQuery, ExportacaoResultadoItem>
    {
        private readonly IRepositorioExportacaoResultadoItem repositorioExportacaoResultadoItem;

        public ObterExportacaoResultadoItemPorProcessoIdDreCodigoQueryHandler(IRepositorioExportacaoResultadoItem repositorioExportacaoResultadoItem)
        {
            this.repositorioExportacaoResultadoItem = repositorioExportacaoResultadoItem ?? throw new ArgumentNullException(nameof(repositorioExportacaoResultadoItem));
        }

        public async Task<ExportacaoResultadoItem> Handle(ObterExportacaoResultadoItemPorProcessoIdDreCodigoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioExportacaoResultadoItem.ObterExportacaoResultadoItemPorProcessoIdDreCodigo(
                request.ProcessoId, request.DreCodigo);
        }
    }
}