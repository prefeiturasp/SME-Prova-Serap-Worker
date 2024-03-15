using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SME.SERAp.Prova.Aplicacao.Commands.ResultadoProvaConsolidado.Incluir
{
    public class InserirResultadoProvaConsolidadoCommand : IRequest<bool>
    {
        public InserirResultadoProvaConsolidadoCommand(Dominio.ResultadoProvaConsolidado resultadoProvaConsolidado)
        {
            ResultadoProvaConsolidado = resultadoProvaConsolidado;
        }

        public Dominio.ResultadoProvaConsolidado ResultadoProvaConsolidado { get; set; }
    }
}

