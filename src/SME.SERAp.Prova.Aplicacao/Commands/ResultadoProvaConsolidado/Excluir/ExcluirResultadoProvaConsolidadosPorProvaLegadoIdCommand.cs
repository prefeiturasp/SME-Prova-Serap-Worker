using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Aplicacao.Commands.ResultadoProvaConsolidado.Excluir
{
    public class ExcluirResultadoProvaConsolidadosPorProvaLegadoIdCommand : IRequest<bool>
    {
        public ExcluirResultadoProvaConsolidadosPorProvaLegadoIdCommand(long provaLegadoId)
        {
            ProvaLegadoId = provaLegadoId;
        }

        public long ProvaLegadoId { get; set; }
    }
}