using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExcluirDadosConsolidadoCommand : IRequest<int>
    {
        public ExcluirDadosConsolidadoCommand(long provaLegadoId, int take, int skip)
        {
            ProvaLegadoId = provaLegadoId;
            Take = take;
            Skip = skip;
        }

        public long ProvaLegadoId { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
    }
}
