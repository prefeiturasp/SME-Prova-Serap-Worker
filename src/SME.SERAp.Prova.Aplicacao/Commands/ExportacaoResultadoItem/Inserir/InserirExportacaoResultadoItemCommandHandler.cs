using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class InserirExportacaoResultadoItemCommandHandler : IRequestHandler<InserirExportacaoResultadoItemCommand, long>
    {
        private readonly IRepositorioExportacaoResultadoItem repositorioExportacaoResultadoItem;
        public InserirExportacaoResultadoItemCommandHandler(IRepositorioExportacaoResultadoItem repositorioExportacaoResultadoItem)
        {
            this.repositorioExportacaoResultadoItem = repositorioExportacaoResultadoItem ?? throw new ArgumentNullException(nameof(repositorioExportacaoResultadoItem));
        }
        public async Task<long> Handle(InserirExportacaoResultadoItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await repositorioExportacaoResultadoItem.IncluirAsync(request.Item);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
