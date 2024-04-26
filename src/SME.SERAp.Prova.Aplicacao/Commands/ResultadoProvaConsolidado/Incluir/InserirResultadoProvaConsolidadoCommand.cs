using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class InserirResultadoProvaConsolidadoCommand : IRequest<bool>
    {
        public InserirResultadoProvaConsolidadoCommand(ResultadoProvaConsolidado resultadoProvaConsolidado)
        {
            ResultadoProvaConsolidado = resultadoProvaConsolidado;
        }

        public ResultadoProvaConsolidado ResultadoProvaConsolidado { get; }
    }
}

