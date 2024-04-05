using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterExportacaoResultadoItemPorIdQueryHandler : IRequestHandler<ObterExportacaoResultadoItemPorIdQuery, ExportacaoResultadoItem>
    {
        private readonly IRepositorioExportacaoResultadoItem repositorioExportacaoResultadoItem;

        public ObterExportacaoResultadoItemPorIdQueryHandler(IRepositorioExportacaoResultadoItem repositorioExportacaoResultadoItem)
        {
            this.repositorioExportacaoResultadoItem = repositorioExportacaoResultadoItem ?? throw new ArgumentNullException(nameof(repositorioExportacaoResultadoItem));
        }

        public async Task<ExportacaoResultadoItem> Handle(ObterExportacaoResultadoItemPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioExportacaoResultadoItem.ObterPorIdAsync(request.Id);
        }
    }
}