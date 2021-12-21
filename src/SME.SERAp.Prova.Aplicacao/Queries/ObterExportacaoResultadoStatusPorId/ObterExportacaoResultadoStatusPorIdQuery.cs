using MediatR;
using SME.SERAp.Prova.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterExportacaoResultadoStatusPorIdQuery : IRequest<ExportacaoResultado>
    {
        public ObterExportacaoResultadoStatusPorIdQuery(long id)
        {
            Id = id;
        }
        public long Id { get; set; }
    }
}
