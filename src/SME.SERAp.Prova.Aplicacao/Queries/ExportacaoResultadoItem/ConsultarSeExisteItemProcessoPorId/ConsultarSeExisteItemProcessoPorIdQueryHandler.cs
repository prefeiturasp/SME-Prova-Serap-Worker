using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ConsultarSeExisteItemProcessoPorIdQueryHandler : IRequestHandler<ConsultarSeExisteItemProcessoPorIdQuery, bool>
    {
        private readonly IRepositorioExportacaoResultadoItem repositorioExportacaoResultadoItem;
        public ConsultarSeExisteItemProcessoPorIdQueryHandler(IRepositorioExportacaoResultadoItem repositorioExportacaoResultadoItem)
        {
            this.repositorioExportacaoResultadoItem = repositorioExportacaoResultadoItem ?? throw new ArgumentNullException(nameof(repositorioExportacaoResultadoItem));
        }
        public async Task<bool> Handle(ConsultarSeExisteItemProcessoPorIdQuery request, CancellationToken cancellationToken)
            => await repositorioExportacaoResultadoItem.ConsultarSeExisteItemProcessoPorIdAsync(request.Id);
    }
}
