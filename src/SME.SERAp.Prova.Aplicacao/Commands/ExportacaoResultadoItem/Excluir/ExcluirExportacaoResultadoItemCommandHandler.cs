using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExcluirExportacaoResultadoItemCommandHandler : IRequestHandler<ExcluirExportacaoResultadoItemCommand, bool>
    {
        private readonly IRepositorioExportacaoResultadoItem repositorioExportacaoResultadoItem;
        public ExcluirExportacaoResultadoItemCommandHandler(IRepositorioExportacaoResultadoItem repositorioExportacaoResultadoItem)
        {
            this.repositorioExportacaoResultadoItem = repositorioExportacaoResultadoItem ?? throw new ArgumentNullException(nameof(repositorioExportacaoResultadoItem));
        }
        public async Task<bool> Handle(ExcluirExportacaoResultadoItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.ItemId > 0)
                    await repositorioExportacaoResultadoItem.ExcluirExportacaoResultadoItemPorIdAsync(request.ItemId);
                if (request.ProcessoId != null)
                    await repositorioExportacaoResultadoItem.ExcluirItensPorProcessoIdAsync((long)request.ProcessoId);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
