using MediatR;
using SME.SERAp.Prova.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterExportacaoResultadoStatusQuery : IRequest<ExportacaoResultado>
    {
        public ObterExportacaoResultadoStatusQuery(long id, long provaSerapId)
        {
            Id = id;
            ProvaSerapId = provaSerapId;
        }

        public long Id { get; set; }
        public long ProvaSerapId { get; set; }
    }
}
