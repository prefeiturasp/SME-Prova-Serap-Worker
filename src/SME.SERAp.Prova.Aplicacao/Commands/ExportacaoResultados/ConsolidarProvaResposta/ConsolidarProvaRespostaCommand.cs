using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ConsolidarProvaRespostaCommand : IRequest<int>
    {
        public ConsolidarProvaRespostaCommand(long provaLegadoId, bool aderirTodos, bool paraEstudanteComDeficiencia, int take, int skip)
        {
            ProvaLegadoId = provaLegadoId;
            AderirTodos = aderirTodos;
            ParaEstudanteComDeficiencia = paraEstudanteComDeficiencia;
            Take = take;
            Skip = skip;
        }

        public long ProvaLegadoId { get; set; }
        public bool AderirTodos { get; set; }
        public bool ParaEstudanteComDeficiencia { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
    }
}
