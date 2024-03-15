using MediatR;

namespace SME.SERAp.Prova.Aplicacao
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

