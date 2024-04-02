using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AlterarExportacaoResultadoItemCommandHandler : IRequestHandler<AlterarExportacaoResultadoItemCommand, bool>
    {
        private readonly IRepositorioExportacaoResultadoItem repositorioExportacaoResultadoItem;

        public AlterarExportacaoResultadoItemCommandHandler(IRepositorioExportacaoResultadoItem repositorioExportacaoResultadoItem)
        {
            this.repositorioExportacaoResultadoItem = repositorioExportacaoResultadoItem ?? throw new ArgumentNullException(nameof(repositorioExportacaoResultadoItem));
        }

        public async Task<bool> Handle(AlterarExportacaoResultadoItemCommand request, CancellationToken cancellationToken)
        {
            await repositorioExportacaoResultadoItem.UpdateAsync(request.Item);
            return true;
        }
    }
}