using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class InserirExportacaoResultadoItemCommand : IRequest<long>
    {
        public InserirExportacaoResultadoItemCommand(ExportacaoResultadoItem item)
        {
            Item = item;
        }

        public ExportacaoResultadoItem Item { get; set; }
    }
}


