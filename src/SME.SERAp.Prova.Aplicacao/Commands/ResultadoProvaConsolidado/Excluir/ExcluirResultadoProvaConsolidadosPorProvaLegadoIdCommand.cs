using MediatR;

namespace SME.SERAp.Prova.Aplicacao
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